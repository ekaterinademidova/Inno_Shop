using UsersApplication.Interfaces.Data;
using UsersApplication.Interfaces.RepositoryContracts;

namespace UsersInfrastructure.Repositories
{
    public class OperationTokenRepository(IApplicationDbContext dbContext)
        : Repository<OperationToken, OperationTokenId>(dbContext), IOperationTokenRepository
    {
    }
}
