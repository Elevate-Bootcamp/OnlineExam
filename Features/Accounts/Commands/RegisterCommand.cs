using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using OnlineExam.Domain;
using OnlineExam.Features.Accounts.Dtos;
using OnlineExam.Shared.Helpers;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Accounts.Commands
{
    public record RegisterCommand(RegisterDto RegisterDto) : IRequest<ServiceResponse<bool>>
    {
        public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ServiceResponse<bool>>
        {
            private readonly JWT _jwtOptions;
            private readonly UserManager<ApplicationUser> userManager;
            private readonly ILogger<RegisterCommand> _logger;
            public RegisterCommandHandler(UserManager<ApplicationUser> userManager
                , JWT jwtoptions,
                  ILogger<RegisterCommand> logger)
            {
                _jwtOptions = jwtoptions;
                this.userManager = userManager;
                _logger = logger;
            }
            public async Task<ServiceResponse<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                // Check if email is already registered
                if (await userManager.FindByEmailAsync(request.RegisterDto.Email) is not null)
                {
                    return ServiceResponse<bool>.ConflictResponse(
                        "Email is already registered");
                }

                // Check if username is already registered
                if (await userManager.FindByNameAsync(request.RegisterDto.UserName) is not null)
                {
                    return ServiceResponse<bool>.ConflictResponse(
                        "Username is already registered");
                }
                var user = new ApplicationUser
                {
                    Email = request.RegisterDto.Email,
                    UserName = request.RegisterDto.UserName,
                    FullName = request.RegisterDto.FullName,
                    EmailConfirmed = false
                };
                var result = await userManager.CreateAsync(user, request.RegisterDto.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return ServiceResponse<bool>.ValidationErrorResponse(
                        new Dictionary<string, List<string>> { { "Password", errors } },
                        "Registration failed");
                }

                // Add user to default role
                await userManager.AddToRoleAsync(user, "User");

                // Get the user ID for verification code
                var userId = (user.Id);

                // Create verification code




                _logger.LogInformation("User {Email} registered successfully", user.Email);
                return ServiceResponse<bool>.SuccessResponse(true,
                    "Registration completed successfully. Please check your email for verification code.",
                    "تمت العملية بنجاح تأكد من بريدك لكود التفعيل");
            }
        }
    }
}
