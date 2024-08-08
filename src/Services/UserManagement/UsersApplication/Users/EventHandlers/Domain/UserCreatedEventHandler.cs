using MassTransit;
using Microsoft.FeatureManagement;

namespace UsersApplication.Users.EventHandlers.Domain
{
    public class UserCreatedEventHandler
        (IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILogger<UserCreatedEventHandler> logger)
        : INotificationHandler<UserCreatedEvent>
    {
        public async Task Handle(UserCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);


            if (await featureManager.IsEnabledAsync("UserFulfillment"))
            {
                var userCreatedIntegrationEvent = domainEvent.user.ToUserDto();
                await publishEndpoint.Publish(userCreatedIntegrationEvent, cancellationToken);
            }
        }
    }
}
