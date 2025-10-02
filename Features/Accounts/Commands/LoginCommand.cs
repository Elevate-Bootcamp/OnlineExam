using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineExam.Domain;
using OnlineExam.Features.Accounts.Dtos;
using OnlineExam.Shared.Helpers;
using OnlineExam.Shared.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineExam.Features.Accounts.Commands
{
    public record LoginCommand(LoginReqDTO LoginDTO) : IRequest<ServiceResponse<UserDto>>;

    public class LoginCommandHandler : IRequestHandler<LoginCommand, ServiceResponse<UserDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWT _jwtOptions;

        public LoginCommandHandler(IOptions<JWT> jwtOptions , UserManager<ApplicationUser> userManager)
        {
            _jwtOptions = jwtOptions.Value;
            _userManager = userManager;
        }

        public async Task<ServiceResponse<UserDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {

            if (request.LoginDTO == null || string.IsNullOrEmpty(request.LoginDTO.UserName) || string.IsNullOrEmpty(request.LoginDTO.Password))
            {
                return ServiceResponse<UserDto>.ErrorResponse("Invalid login request", "طلب تسجيل دخول غير صالح", 400);
            }
            var user = await _userManager.FindByNameAsync(request.LoginDTO.UserName);
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
            
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, request.LoginDTO.UserName),
            new Claim(ClaimTypes.Name, request.LoginDTO.UserName),
            new Claim(ClaimTypes.Email, "a@b.com"),
            new Claim(ClaimTypes.Role, "user")
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secretkey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,           // Must match ValidIssuer
                audience: _jwtOptions.Audience,         // Must match ValidAudience
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var userdto = new UserDto
            {
                IsAuthenticated = true,
                Username = user.UserName,
                Email = user.Email,
                Roles = new List<string> { "user" },
                Token = tokenString,
                //Expiration = token.ValidTo,
                RefreshToken = null, // Implement refresh token logic as needed
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(7), // Example expiration
                EmailConfirmed = user.EmailConfirmed
            };


            return ServiceResponse<UserDto>.SuccessResponse(userdto);
        }
    }
}