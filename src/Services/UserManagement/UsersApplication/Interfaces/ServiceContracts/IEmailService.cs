namespace UsersApplication.Interfaces.ServiceContracts
{
    public interface IEmailService
    {        
        Task SendEmailConfirmationAsync(User user, CancellationToken cancellationToken);
        Task SendPasswordResetAsync(User user, CancellationToken cancellationToken);
    }
}
