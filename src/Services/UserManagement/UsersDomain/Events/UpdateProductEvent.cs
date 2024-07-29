namespace UsersDomain.Events
{
    public record UpdateProductEvent(User user, Product product) : IDomainEvent;
}