namespace UsersApplication.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendPasswordResetAsync(User user, CancellationToken cancellationToken);
        Task SendEmailConfirmationAsync(User user, CancellationToken cancellationToken);
    }
}
