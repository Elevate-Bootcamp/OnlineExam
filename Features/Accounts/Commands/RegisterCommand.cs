using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using OnlineExam.Domain;
using OnlineExam.Features.Accounts.Dtos;

namespace OnlineExam.Features.Accounts.Commands
{
    public record RegisterCommand(string UserName, string Email, string FullName, string Password) : IRequest<UserDto>
    {
        public class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserDto>
        {
            private readonly UserManager<ApplicationUser> userManager;
            public RegisterCommandHandler(UserManager<ApplicationUser> userManager)
            {
                this.userManager = userManager;
            }
            public async Task<UserDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                var user = new ApplicationUser
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    FullName = request.FullName,
                    SecurityStamp = Guid.NewGuid().ToString()
                    // Do not set PasswordHash manually; UserManager will handle it
                };
                var result = await userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    return new UserDto
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        FullName = user.FullName,
                    };
                }
                return null;

            }
        }
    }
}
