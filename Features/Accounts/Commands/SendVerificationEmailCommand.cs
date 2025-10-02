using MailKit.Net.Smtp;
using MailKit.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using OnlineExam.Domain;
using OnlineExam.Domain.Entities;
using OnlineExam.Features.Accounts.Commands;
using OnlineExam.Infrastructure.ApplicationDBContext;
using OnlineExam.Shared.Helpers;
using OnlineExam.Shared.Responses;
using System.Text.Json;

namespace OnlineExam.Features.Accounts.Commands
{
    public record SendVerificationEmailCommand(string UserId, string Email, string? ConfirmationCode = null) : IRequest<ServiceResponse<bool>>
    {
        public class Handler : IRequestHandler<SendVerificationEmailCommand, ServiceResponse<bool>>
        {
            private readonly EmailSettings _emailSettings;
            private readonly ApplicationDbContext _context; // Assuming your DbContext
            private readonly ILogger<SendVerificationEmailCommand> _logger;

            public Handler(IOptions<EmailSettings> emailSettings, ApplicationDbContext context, ILogger<SendVerificationEmailCommand> logger)
            {
                _emailSettings = emailSettings.Value;
                _context = context;
                _logger = logger;
            }

            public async Task<ServiceResponse<bool>> Handle(SendVerificationEmailCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var code = request.ConfirmationCode ?? GenerateVerificationCode();
                    var subject = "Confirm Your Email - OnlineExam";
                    var body = GetEmailConfirmationTemplate(code);

                    // Queue the email (adapted from your old QueueEmailAsync)
                    await QueueEmailAsync(request.Email, subject, body, EmailType.Verification, EmailPriority.High, new { Code = code }, true, null, "User");

                    // Optional: Send directly for immediate processing (or use background service for batching)
                    var pendingEmail = await _context.emailQueues
                        .Where(e => e.Status == EmailStatus.Pending && e.ToEmail == request.Email)
                        .OrderByDescending(e => e.CreatedAt)
                        .FirstOrDefaultAsync();

                    if (pendingEmail != null)
                    {
                        var sendSuccess = await SendEmailDirectlyAsync(pendingEmail);
                        if (sendSuccess)
                        {
                            await UpdateEmailStatusAsync(pendingEmail.Id, EmailStatus.Sent);
                            _logger.LogInformation("Verification email sent to {Email}", request.Email);
                            return ServiceResponse<bool>.SuccessResponse(true, "Confirmation email sent");
                        }
                        else
                        {
                            await UpdateEmailStatusAsync(pendingEmail.Id, EmailStatus.Failed, "SMTP send failed");
                            return ServiceResponse<bool>.InternalServerErrorResponse("Failed to send confirmation email");
                        }
                    }

                    return ServiceResponse<bool>.SuccessResponse(true, "Email queued successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending email confirmation to {Email}", request.Email);
                    return ServiceResponse<bool>.InternalServerErrorResponse("Failed to send confirmation email");
                }
            }

            // Adapted from your old QueueEmailAsync
            private async Task QueueEmailAsync(string toEmail, string subject, string body, EmailType emailType, EmailPriority priority = EmailPriority.Normal,
                object? templateData = null, bool isHtml = true, DateTime? scheduledAt = null, string? toName = null)
            {
                var emailQueue = new EmailQueue
                {
                    Id = Guid.NewGuid(),
                    ToEmail = toEmail,
                    Subject = subject,
                    ToName = toName ?? string.Empty,
                    Body = body,
                    IsHtml = isHtml,
                    EmailType = emailType,
                    Priority = priority,
                    ScheduledAt = scheduledAt ?? DateTime.UtcNow,
                    TemplateData = templateData != null ? JsonSerializer.Serialize(templateData) : string.Empty,
                    CreatedAt = DateTime.UtcNow,
                    Status = EmailStatus.Pending,
                    ErrorMessage = string.Empty,
                    MaxRetries = 3,
                    RetryCount = 0
                };

                await _context.emailQueues.AddAsync(emailQueue);
                await _context.SaveChangesAsync();
            }

            // Adapted from your old SendEmailDirectlyAsync
            // In SendVerificationEmailCommand.Handler.SendEmailDirectlyAsync
            private async Task<bool> SendEmailDirectlyAsync(EmailQueue emailItem)
            {
                try
                {
                    _logger.LogInformation("Starting email send for {Email}. Server: {Server}, Port: {Port}, SSL: {Ssl}",
                        emailItem.ToEmail, _emailSettings.SmtpServer, _emailSettings.Port, _emailSettings.EnableSsl);

                    if (string.IsNullOrEmpty(_emailSettings.SmtpServer) || string.IsNullOrEmpty(_emailSettings.Username))
                    {
                        _logger.LogError("Email config incomplete: Server={Server}, Username={Username}",
                            _emailSettings.SmtpServer ?? "NULL", _emailSettings.Username ?? "NULL");
                        return false;
                    }

                    using var emailMessage = new MimeMessage();
                    emailMessage.From.Add(new MailboxAddress(_emailSettings.FromName ?? "OnlineExam", _emailSettings.FromEmail));
                    emailMessage.To.Add(new MailboxAddress(emailItem.ToName ?? "", emailItem.ToEmail));
                    emailMessage.Subject = emailItem.Subject;

                    var builder = new BodyBuilder();
                    if (emailItem.IsHtml)
                    {
                        builder.HtmlBody = emailItem.Body;
                    }
                    else
                    {
                        builder.TextBody = emailItem.Body;
                    }
                    emailMessage.Body = builder.ToMessageBody();

                    using var client = new SmtpClient();

                    // Gmail-specific SSL options
                    SecureSocketOptions secureOptions;
                    if (_emailSettings.Port == 587)
                    {
                        secureOptions = SecureSocketOptions.StartTls;  // Recommended for Gmail
                        _logger.LogInformation("Using StartTls for Port 587");
                    }
                    else if (_emailSettings.Port == 465)
                    {
                        secureOptions = SecureSocketOptions.SslOnConnect;  // For implicit SSL
                        _logger.LogInformation("Using SslOnConnect for Port 465");
                    }
                    else
                    {
                        secureOptions = SecureSocketOptions.Auto;
                        _logger.LogInformation("Using Auto for Port {_Port}", _emailSettings.Port);
                    }

                    await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, secureOptions);
                    _logger.LogInformation("Connected to SMTP server");

                    await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                    _logger.LogInformation("SMTP authentication successful");

                    await client.SendAsync(emailMessage);
                    _logger.LogInformation("Email sent to {Email}", emailItem.ToEmail);

                    await client.DisconnectAsync(true);
                    return true;
                }
                catch (AuthenticationException authEx)
                {
                    _logger.LogError(authEx, "SMTP Authentication failed for {Email}. Check app password.", emailItem.ToEmail);
                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "SMTP error for {Email}: {Message}", emailItem.ToEmail, ex.Message);
                    return false;
                }
            }
            // Adapted from your old UpdateEmailStatusAsync
            private async Task UpdateEmailStatusAsync(Guid emailId, EmailStatus status, string? errorMessage = null)
            {
                var email = await _context.emailQueues.FindAsync(emailId);
                if (email != null)
                {
                    email.Status = status;
                    email.ErrorMessage = errorMessage;

                    if (status == EmailStatus.Sent)
                    {
                        email.SentAt = DateTime.UtcNow;
                    }
                    else if (status == EmailStatus.Failed)
                    {
                        email.RetryCount++;
                        if (email.RetryCount < email.MaxRetries)
                        {
                            email.Status = EmailStatus.Pending;
                            email.NextRetryAt = DateTime.UtcNow.AddMinutes((int)Math.Pow(2, email.RetryCount)); // Exponential backoff
                        }
                        else
                        {
                            email.Status = EmailStatus.Failed;
                        }
                    }

                    await _context.SaveChangesAsync();
                }
            }

            // Adapted from your old GetEmailConfirmationTemplate
            private string GetEmailConfirmationTemplate(string confirmationCode)
            {
                return $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='utf-8'>
                        <title>Email Verification</title>
                        <style>
                            body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                            .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                            .code-box {{ background-color: #f8f9fa; border: 2px solid #007bff; padding: 20px; text-align: center; font-size: 32px; font-weight: bold; color: #007bff; border-radius: 10px; margin: 20px 0; letter-spacing: 3px; }}
                            .footer {{ margin-top: 30px; font-size: 12px; color: #666; }}
                            .warning {{ background-color: #fff3cd; border: 1px solid #ffeaa7; padding: 10px; border-radius: 5px; margin: 15px 0; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h2>OnlineExam - Email Verification</h2>
                            <p>Thank you for registering with OnlineExam. Please use the verification code below to confirm your email address:</p>
                            
                            <div class='code-box'>
                                {confirmationCode}
                            </div>
                            
                            <div class='warning'>
                                <strong>Important:</strong> This verification code will expire in 24 hours.
                            </div>
                            
                            <p>Enter this code in the verification form to complete your registration.</p>
                            <p>If you didn't create an account with OnlineExam, please ignore this email.</p>
                            
                            <div class='footer'>
                                <p>This is an automated email. Please do not reply.</p>
                                <p>&copy; 2025 OnlineExam. All rights reserved.</p>
                            </div>
                        </div>
                    </body>
                    </html>";
            }

            // Adapted from your old GenerateVerificationCode
            private string GenerateVerificationCode()
            {
                const string numbers = "0123456789";
                var code = new char[6];
                var random = new Random();
                for (int i = 0; i < 6; i++)
                {
                    code[i] = numbers[random.Next(numbers.Length)];
                }
                return new string(code);
            }
        }
    }
}