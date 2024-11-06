using MimeKit;

namespace UsersApplication.Interfaces.WrappersContracts
{
    public interface ISmtpClientWrapper
    {
        Task ConnectAsync(string host, int port, bool useSsl, CancellationToken cancellationToken);
        Task AuthenticateAsync(string username, string password, CancellationToken cancellationToken);
        Task SendAsync(MimeMessage message, CancellationToken cancellationToken);
        Task DisconnectAsync(bool quit, CancellationToken cancellationToken);
    }
}
