using MassTransit;
using Microsoft.FeatureManagement;
using UsersApplication.Interfaces.ServiceContracts;
using UsersDomain.Events;

namespace UsersApplication.Users.EventHandlers.Domain
{
    public class UserCreatedEventHandler
        (IEmailService emailService, ILogger<UserCreatedEventHandler> logger)
        : INotificationHandler<UserCreatedEvent>
    {
        public async Task Handle(UserCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

            await emailService.SendEmailConfirmationAsync(domainEvent.User, cancellationToken);
        }

    }
}
