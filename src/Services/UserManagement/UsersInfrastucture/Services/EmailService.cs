using MimeKit;
using UsersApplication.Interfaces;
using UsersApplication.Interfaces.ServiceContracts;
using UsersApplication.Interfaces.WrappersContracts;
using UsersApplication.ValueObjects;

namespace UsersInfrastructure.Services
{
    internal record EmailDetails(OperationType OperationType, string PathSegment, string Subject, string Description);

    public class EmailService(IEmailSettings emailSettings, IUnitOfWork unitOfWork, ISmtpClientWrapper smtpClientWrapper) : IEmailService
    {
        private static readonly EmailDetails EmailConfirmationDetails = new(
              OperationType.EmailConfirmation,
              "confirm-email",
              "Please confirm your email",
              "Please confirm your email by clicking this link");

        private static readonly EmailDetails PasswordResetDetails = new(
            OperationType.PasswordReset,
            "reset-password",
            "Password reset",
            "Reset your password by clicking this link");

        public Task SendEmailConfirmationAsync(User user, CancellationToken cancellationToken)
        {
            return HandleEmailAsync(user, EmailConfirmationDetails, cancellationToken);
        }

        public Task SendPasswordResetAsync(User user, CancellationToken cancellationToken)
        {
            return HandleEmailAsync(user, PasswordResetDetails, cancellationToken);
        }

        private async Task HandleEmailAsync(User user, EmailDetails emailDetails, CancellationToken cancellationToken)
        {
            var token = await GenerateToken(user.Id, emailDetails.OperationType, cancellationToken);
            if (string.IsNullOrEmpty(token))
                throw new InvalidOperationException("Error generating token.");

            var link = GenerateLink(emailDetails.PathSegment, token);
            var message = CreateMimeMessage(user, link, emailDetails.Subject, emailDetails.Description);

            await SendEmailAsync(message, cancellationToken);
        }

        public async Task<string> GenerateToken(UserId userId, OperationType operationType, CancellationToken cancellationToken)
        {
            var operationTokenCode = Guid.NewGuid();
            var token = OperationToken.Create(userId, operationTokenCode, operationType, 1440);
            await unitOfWork.OperationToken.AddAsync(token, cancellationToken);
            await unitOfWork.SaveAsync(cancellationToken);

            return token.Code.ToString();
        }

        public static string GenerateLink(string pathSegment, string token)
        {
            return $"https://localhost:5051/users/{pathSegment}?token={token}";  // must to be changed for docker     
        }

        public MimeMessage CreateMimeMessage(User user, string link, string subject, string description)
        {
            var message = new MimeMessage
            {
                From = { new MailboxAddress(emailSettings.SenderName, emailSettings.SenderEmail) },
                To = { new MailboxAddress(user.FullName, user.Email) },
                Subject = subject,
                Body = new BodyBuilder
                {
                    HtmlBody = $"{description}: <a href=\"{link}\">{link}</a>"
                }.ToMessageBody()
            };

            return message;
        }

        public async Task SendEmailAsync(MimeMessage message, CancellationToken cancellationToken)
        {
            try
            {
                await smtpClientWrapper.ConnectAsync(emailSettings.SmtpServer, emailSettings.SmtpPort, emailSettings.UseSsl, cancellationToken);
                await smtpClientWrapper.AuthenticateAsync(emailSettings.SmtpName, emailSettings.SmtpPassword, cancellationToken);
                await smtpClientWrapper.SendAsync(message, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error sending email: " + ex.Message, ex);
            }
            finally
            {
                await smtpClientWrapper.DisconnectAsync(true, cancellationToken);
            }
        }
    }
}
