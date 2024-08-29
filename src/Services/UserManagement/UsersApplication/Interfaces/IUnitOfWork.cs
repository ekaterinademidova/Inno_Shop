using UsersApplication.Interfaces.Repositories;

namespace UsersApplication.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        IOperationTokenRepository OperationToken { get; }
        void Save();
        Task SaveAsync(CancellationToken cancellationToken);
    }
}
