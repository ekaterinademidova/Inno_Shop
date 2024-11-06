namespace UsersDomain.Events
{
    public record UserUpdatedEvent(User User) : IDomainEvent;
}
