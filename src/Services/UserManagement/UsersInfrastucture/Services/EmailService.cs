using MimeKit;
using UsersApplication.Interfaces;
using UsersApplication.Models;
using MailKit.Net.Smtp;
using UsersApplication.Interfaces.Services;

namespace UsersInfrastucture.Services
{
    public class EmailService(EmailSettings emailSettings, IUnitOfWork unitOfWork) : IEmailService
    {
        public async Task SendPasswordResetAsync(User user, CancellationToken cancellationToken)
        {
            var token = GeneratePasswordResetToken(user.Id);
            if (string.IsNullOrEmpty(token))
                throw new InvalidOperationException("Error generating password reset token.");

            var link = GeneratePasswordResetLink(token);

            var message = CreateMimeMessage2(user, link);

            await SendEmailAsync(message, cancellationToken);
        }

        public async Task SendEmailConfirmationAsync(User user, CancellationToken cancellationToken)
        {
            var token = GenerateEmailConfirmationToken(user.Id);
            if (string.IsNullOrEmpty(token)) 
                throw new InvalidOperationException("Error generating email confirmation token.");

            var link = GenerateEmailConfirmationLink(token);

            var message = CreateMimeMessage(user, link);

            await SendEmailAsync(message, cancellationToken);
        }
        private string GeneratePasswordResetToken(UserId userId)
        {
            var token = OperationToken.Create(userId, OperationType.PasswordReset, 1440);
            unitOfWork.OperationToken.Add(token);
            unitOfWork.Save();

            return token.Code.ToString();
        }

        private string GenerateEmailConfirmationToken(UserId userId)
        {
            var token = OperationToken.Create(userId, OperationType.EmailConfirmation, 1440);
            unitOfWork.OperationToken.Add(token);
            unitOfWork.Save();

            return token.Code.ToString();
        }

        private static string GeneratePasswordResetLink(string token)
        {
            return $"https://localhost:5051/users/reset-password?token={token}";
        }

        private static string GenerateEmailConfirmationLink(string token)
        {
            return $"https://localhost:5051/users/confirm-email?token={token}";
        }

        private MimeMessage CreateMimeMessage2(User user, string link)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailSettings.SenderName, emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress(user.FullName, user.Email));

            message.Subject = "Password reset";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"Reset your password by clicking this link: <a href=\"{link}\">{link}</a>"
            };
            message.Body = bodyBuilder.ToMessageBody();

            return message;
        }

        private MimeMessage CreateMimeMessage(User user, string link)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailSettings.SenderName, emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress(user.FullName, user.Email));

            message.Subject = "Please confirm your email";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"Please confirm your email by clicking this link: <a href=\"{link}\">{link}</a>"
            };
            message.Body = bodyBuilder.ToMessageBody();

            return message;
        }

        private async Task SendEmailAsync(MimeMessage message, CancellationToken cancellationToken)
        {
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(emailSettings.SmtpServer, emailSettings.SmtpPort, emailSettings.UseSsl, cancellationToken);
                await client.AuthenticateAsync(emailSettings.SmtpName, emailSettings.SmtpPassword, cancellationToken);
                await client.SendAsync(message, cancellationToken);
            }
            catch (Exception ex)
            {
                // Логирование или обработка ошибок
                throw new InvalidOperationException("Error sending email: " + ex.Message, ex);
            }
            finally
            {
                await client.DisconnectAsync(true, cancellationToken);
            }
        }
    }
}
