using MassTransit;
using Microsoft.FeatureManagement;
using UsersApplication.Interfaces.Services;
using UsersDomain.Events;

namespace UsersApplication.Users.EventHandlers.Domain
{
    public class UserCreatedEventHandler
        (/*IPublishEndpoint publishEndpoint, IFeatureManager featureManager,*/ IEmailService emailService, ILogger<UserCreatedEventHandler> logger)
        : INotificationHandler<UserCreatedEvent>
    {
        public async Task Handle(UserCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

            await emailService.SendEmailConfirmationAsync(domainEvent.user, cancellationToken);

            //return Task.CompletedTask;

            //if (await featureManager.IsEnabledAsync("UserFulfillment"))
            //{
            //    var userCreatedIntegrationEvent = domainEvent.user.ToUserDto();
            //    await publishEndpoint.Publish(userCreatedIntegrationEvent, cancellationToken);
            //}
        }

    }
}
