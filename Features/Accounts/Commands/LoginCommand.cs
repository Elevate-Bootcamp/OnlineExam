using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineExam.Domain;
using OnlineExam.Domain.Entities;
using OnlineExam.Features.Accounts.Dtos;
using OnlineExam.Shared.Helpers;
using OnlineExam.Shared.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OnlineExam.Features.Accounts.Commands
{
    public record LoginCommand(LoginReqDTO LoginDTO) : IRequest<ServiceResponse<UserDto>>;

    public class LoginCommandHandler : IRequestHandler<LoginCommand, ServiceResponse<UserDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWT _jwtOptions;

        public LoginCommandHandler(IOptions<JWT> jwtOptions, UserManager<ApplicationUser> userManager)
        {
            _jwtOptions = jwtOptions.Value;
            _userManager = userManager;
        }

        public async Task<ServiceResponse<UserDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (request.LoginDTO == null || string.IsNullOrEmpty(request.LoginDTO.Email) || string.IsNullOrEmpty(request.LoginDTO.Password))
            {
                return ServiceResponse<UserDto>.ErrorResponse("Invalid login request", "طلب تسجيل دخول غير صالح", 400);
            }

            // 🔹 Changed: Find user by email instead of username
            var user = await _userManager.FindByEmailAsync(request.LoginDTO.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.LoginDTO.Password))
            {
                return ServiceResponse<UserDto>.UnauthorizedResponse();
            }

            // Optional: Check if email is confirmed
            if (!user.EmailConfirmed)
            {
                return ServiceResponse<UserDto>.ErrorResponse(
                    "Email not confirmed. Please check your email and confirm your account.",
                    "البريد الإلكتروني غير مؤكد. يرجى التحقق من بريدك الإلكتروني وتأكيد حسابك.",
                    403);
            }

            // 🔹 Changed: Use email for claims instead of username
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id), // Use user ID instead of username
                new Claim(ClaimTypes.Name, user.UserName ?? user.Email), // Use username if exists, otherwise email
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "user")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secretkey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),  // JWT expires in 1 hour
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Refresh Token Logic
            var refreshToken = GetOrGenerateRefreshToken(user);
            await _userManager.UpdateAsync(user);  // Save if new token added

            var userDto = new UserDto
            {
                IsAuthenticated = true,
                Username = user.UserName ?? user.Email, // Use email if username is null
                Email = user.Email,
                Roles = new List<string> { "user" },
                Token = tokenString,
                RefreshToken = refreshToken.Token,  // Include in response
                RefreshTokenExpiration = refreshToken.ExpiresOn,  // Include expiration
                EmailConfirmed = user.EmailConfirmed
            };

            return ServiceResponse<UserDto>.SuccessResponse(userDto);
        }

        // Private method to generate a secure refresh token
        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),  // 10-day expiration
                CreatedOn = DateTime.UtcNow
            };
        }

        // Private method to get existing active token or generate new one
        private RefreshToken GetOrGenerateRefreshToken(ApplicationUser user)
        {
            // Check for existing active refresh token
            var activeRefreshToken = user.RefreshTokens?.FirstOrDefault(t => t.IsActive);
            if (activeRefreshToken != null)
            {
                return activeRefreshToken;  // Reuse existing
            }

            // Generate new one
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens ??= new List<RefreshToken>();  // Initialize if null
            user.RefreshTokens.Add(newRefreshToken);

            return newRefreshToken;
        }
    }
}