namespace UsersDomain.Events
{
    public record DeleteProductEvent(User user, ProductId productId) : IDomainEvent;
}