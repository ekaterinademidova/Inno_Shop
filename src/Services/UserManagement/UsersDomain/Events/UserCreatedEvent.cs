namespace UsersDomain.Events
{
    public record UserCreatedEvent(User User) : IDomainEvent;
}
