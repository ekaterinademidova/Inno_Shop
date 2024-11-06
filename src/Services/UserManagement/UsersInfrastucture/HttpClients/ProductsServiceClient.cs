using System.Net.Http.Json;
using UsersApplication.Dtos;
using UsersApplication.Interfaces.HttpClientContracts;

namespace UsersInfrastructure.HttpClients
{
    public class ProductsServiceClient(HttpClient client) : IProductsServiceClient
    {
        private const string JsonErrorMessage = "Failed to deserialize the response or the response is null.";
        private const string ServiceErrorMessage = "Service error.";
        private const string NetworkErrorMessage = "Network error.";
        private const string TimeoutErrorMessage = "The request timed out.";

        public async Task<Guid> CreateProductAsync(ProductDto productDto)
        {
            var response = await ExecuteAsync(() => client.PostAsJsonAsync("/products", productDto));
            ProductDto? product = await DeserializeResponseAsync<ProductDto>(response);
            return product.Id;
        }

        public async Task<bool> UpdateProductAsync(ProductDto productDto)
        {
            var response = await ExecuteAsync(() => client.PutAsJsonAsync("/products", productDto));
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProductAsync(Guid productId)
        {
            var response = await ExecuteAsync(() => client.DeleteAsync($"/products/{productId}"));
            return response.IsSuccessStatusCode;
        }

        private static async Task<HttpResponseMessage> ExecuteAsync(Func<Task<HttpResponseMessage>> operation)
        {
            try
            {
                var response = await operation();
                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException($"{ServiceErrorMessage} Status code: {response.StatusCode}");
                }
                return response;
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException(NetworkErrorMessage, ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new TimeoutException(TimeoutErrorMessage, ex);
            }
        }

        private static async Task<T> DeserializeResponseAsync<T>(HttpResponseMessage response)
        {
            T? result = await response.Content.ReadFromJsonAsync<T>()
               ?? throw new InvalidOperationException(JsonErrorMessage);

            return result;
        }
    }
}
