using UsersApplication.Interfaces;
using UsersApplication.Interfaces.Data;
using UsersApplication.Interfaces.Repositories;
using UsersInfrastucture.Repositories;

namespace UsersInfrastucture
{
    public class UnitOfWork : IUnitOfWork
    {
        private IApplicationDbContext _dbContext;
        public IUserRepository User { get; private set; }
        public IOperationTokenRepository OperationToken { get; private set; }

        public UnitOfWork(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            User = new UserRepository(_dbContext);
            OperationToken = new OperationTokenRepository(_dbContext);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
