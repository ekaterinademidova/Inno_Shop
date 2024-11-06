namespace UsersApplication.Interfaces.HttpClientContracts
{
    public interface IProductsServiceClient
    {
        Task<Guid> CreateProductAsync(ProductDto productDto);
        Task<bool> UpdateProductAsync(ProductDto productDto);
        Task<bool> DeleteProductAsync(Guid productId);
    }
}
