using System.Net.Http.Json;
using UsersApplication.Dtos;
using UsersApplication.Interfaces.HttpClients;

namespace UsersInfrastucture.HttpClients
{
    public class ProductsServiceClient(HttpClient client) : IProductsServiceClient
    {
        private readonly string errorMessage = "Failed to deserialize the response or the response is null.";

        public async Task<Guid> CreateProductAsync(ProductDto productDto)
        {
            var response = await client.PostAsJsonAsync("/products", productDto);
            response.EnsureSuccessStatusCode();
            ProductDto? product = await response.Content.ReadFromJsonAsync<ProductDto>()
                 ?? throw new InvalidOperationException(errorMessage);
            return product.Id;
        }

        public async Task<bool> UpdateProductAsync(ProductDto productDto)
        {
            var response = await client.PutAsJsonAsync("/products", productDto);
            response.EnsureSuccessStatusCode();
            //SuccessResponseDto? result = await response.Content.ReadFromJsonAsync<SuccessResponseDto>()
            //     ?? throw new InvalidOperationException(errorMessage);
            //return result.IsSuccess;
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteProductAsync(Guid productId)
        {
            var response = await client.DeleteAsync($"/products/{productId}");
            response.EnsureSuccessStatusCode();
            //SuccessResponseDto? result = await response.Content.ReadFromJsonAsync<SuccessResponseDto>()
            //     ?? throw new InvalidOperationException(errorMessage);
            //return result.IsSuccess;
            return response.IsSuccessStatusCode;
        }
    }
}
