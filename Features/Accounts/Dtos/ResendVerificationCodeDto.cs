namespace OnlineExam.Features.Accounts.Dtos
{
    public class ResendVerificationCodeDto
    {
        public string Email { get; set; }
        public string? ConfirmationCode { get; set; } = null;
    }
}
