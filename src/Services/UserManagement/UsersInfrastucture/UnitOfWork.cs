using Microsoft.Extensions.Logging;
using UsersApplication.Interfaces;
using UsersApplication.Interfaces.Data;
using UsersApplication.Interfaces.RepositoryContracts;
using UsersInfrastructure.Repositories;

namespace UsersInfrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<UnitOfWork> _logger;
        public IUserRepository User { get; private set; }
        public IOperationTokenRepository OperationToken { get; private set; }

        public UnitOfWork(IApplicationDbContext dbContext, ILogger<UnitOfWork> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            User = new UserRepository(_dbContext);
            OperationToken = new OperationTokenRepository(_dbContext);
        }

        public async Task<bool> SaveAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (DbUpdateException dbUpdateException)
            {
                _logger.LogError(dbUpdateException, "An error occurred during saving changes.");
                return false;
            }
            
        }
    }
}
