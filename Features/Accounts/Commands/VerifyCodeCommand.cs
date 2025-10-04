using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OnlineExam.Domain;
using OnlineExam.Features.Accounts.Dtos; // ConfirmEmailWithCodeDto here
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Accounts.Commands
{
    public record VerifyCodeCommand(string UserId, string Code, VerificationCodeType CodeType) : IRequest<ServiceResponse<bool>>
    {
        public class Handler : IRequestHandler<VerifyCodeCommand, ServiceResponse<bool>>
        {
            private readonly ILogger<VerifyCodeCommand> _logger;
            // Add dependencies for code verification (e.g., VerificationCodeRepository if needed)

            public Handler(ILogger<VerifyCodeCommand> logger)
            {
                _logger = logger;
            }

            public async Task<ServiceResponse<bool>> Handle(VerifyCodeCommand request, CancellationToken cancellationToken)
            {
                // Implement code verification logic here (e.g., check DB for matching code, expire after time)
                // For example:
                // var storedCode = await _repo.GetCodeAsync(request.UserId, request.CodeType);
                // if (storedCode == null || storedCode.Code != request.Code || storedCode.Expired)
                //     return ServiceResponse<bool>.ErrorResponse(new List<string> { "Invalid or expired code" }, "فشل في التحقق من الكود", 400);

                _logger.LogInformation("Code verified for user {UserId}, type {CodeType}", request.UserId, request.CodeType);
                return ServiceResponse<bool>.SuccessResponse(true, "Code verified successfully", "تم التحقق من الكود بنجاح");
            }
        }
    }
}