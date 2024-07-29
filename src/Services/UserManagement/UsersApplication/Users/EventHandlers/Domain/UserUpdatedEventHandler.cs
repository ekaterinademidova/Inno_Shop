namespace UsersApplication.Users.EventHandlers.Domain
{
    public class UserUpdatedEventHandler(ILogger<UserUpdatedEventHandler> logger)
        : INotificationHandler<UserUpdatedEvent>
    {
        public Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}