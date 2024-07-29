namespace UsersDomain.Events
{
    public record UserCreatedEvent(User user) : IDomainEvent;
}
