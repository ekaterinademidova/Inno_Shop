namespace UsersDomain.Events
{
    public record CreateProductEvent(User user, Product product) : IDomainEvent;
}
