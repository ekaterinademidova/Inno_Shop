using MailKit.Net.Smtp;
using MimeKit;
using UsersApplication.Interfaces.WrappersContracts;

namespace UsersInfrastructure.Wrappers
{
    public class SmtpClientWrapper : ISmtpClientWrapper
    {
        private readonly SmtpClient _smtpClient;

        public SmtpClientWrapper()
        {
            _smtpClient = new SmtpClient();
        }

        public Task ConnectAsync(string host, int port, bool useSsl, CancellationToken cancellationToken)
        {
            return _smtpClient.ConnectAsync(host, port, useSsl, cancellationToken);
        }

        public Task AuthenticateAsync(string username, string password, CancellationToken cancellationToken)
        {
            return _smtpClient.AuthenticateAsync(username, password, cancellationToken);
        }

        public Task SendAsync(MimeMessage message, CancellationToken cancellationToken)
        {
            return _smtpClient.SendAsync(message, cancellationToken);
        }

        public Task DisconnectAsync(bool quit, CancellationToken cancellationToken)
        {
            return _smtpClient.DisconnectAsync(quit, cancellationToken);
        }
    }
}
