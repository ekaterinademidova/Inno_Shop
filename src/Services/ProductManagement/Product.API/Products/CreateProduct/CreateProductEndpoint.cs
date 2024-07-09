namespace Product.API.Products.CreateProduct
{
    public record CreateProductRequest(string Name, string Description, string ImageFile, decimal Price, int Quantity, Guid CreatedByUserId, DateTime CreatedDate, DateTime LastModified);
    public record CreateProductResponse(Guid Id);
    public class CreateProductEndpoint
    {
    }
}