namespace ProductsAPI.Interfaces.HttpClientContracts
{
    public interface IUsersServiceClient
    {
        Task<bool> GetUserByIdAsync(Guid userId);
    }
}
