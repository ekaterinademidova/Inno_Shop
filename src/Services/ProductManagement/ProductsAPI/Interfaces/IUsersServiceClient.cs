namespace ProductsAPI.Interfaces
{
    public interface IUsersServiceClient
    {
        Task<bool> GetUserByIdAsync(Guid userId);
    }
}
