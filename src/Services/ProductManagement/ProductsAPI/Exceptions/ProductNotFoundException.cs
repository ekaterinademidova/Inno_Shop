namespace ProductsAPI.Exceptions
{
    public class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(Guid Id) : base("Product", Id) { }
    }
}
