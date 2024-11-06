using UsersApplication.Interfaces.RepositoryContracts;

namespace UsersApplication.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        IOperationTokenRepository OperationToken { get; }
        Task<bool> SaveAsync(CancellationToken cancellationToken);
    }
}
