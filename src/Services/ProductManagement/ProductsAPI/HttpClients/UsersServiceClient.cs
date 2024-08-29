using ProductsAPI.Interfaces;

namespace ProductsAPI.HttpClients
{
    public class UsersServiceClient(HttpClient client) : IUsersServiceClient
    {
        public async Task<bool> GetUserByIdAsync(Guid userId)
        {
            var response = await client.GetAsync($"/users/{userId}");
            return response.IsSuccessStatusCode;
        }
    }
}
