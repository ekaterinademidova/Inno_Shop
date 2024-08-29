using UsersApplication.Interfaces.Data;
using UsersApplication.Interfaces.Repositories;

namespace UsersInfrastucture.Repositories
{
    public class OperationTokenRepository(IApplicationDbContext dbContext)
        : Repository<OperationToken, OperationTokenId>(dbContext), IOperationTokenRepository
    {
    }
}
