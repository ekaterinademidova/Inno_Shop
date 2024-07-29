namespace UsersDomain.Events
{
    public record UserUpdatedEvent(User user) : IDomainEvent;
}
