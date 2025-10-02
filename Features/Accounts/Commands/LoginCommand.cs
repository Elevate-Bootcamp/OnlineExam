using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineExam.Features.Accounts.Dtos;
using OnlineExam.Shared.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineExam.Features.Accounts.Commands
{
    public record LoginCommand(LoginReqDTO LoginDTO) : IRequest<IResult>;

    public class LoginCommandHandler : IRequestHandler<LoginCommand, IResult>
    {
        private readonly JWT _jwtOptions;

        public LoginCommandHandler(IOptions<JWT> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<IResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // TODO: Add actual user validation from database

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

            return Results.Ok(new { token = tokenString, expiresIn = DateTime.UtcNow.AddHours(1) });
        }
    }
}