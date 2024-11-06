using Microsoft.Extensions.Logging;
using UsersApplication.Interfaces.Data;

namespace UsersInfrastructure.Tests.Fixtures
{
    public static class UnitOfWorkTestFixture
    {
        public static UnitOfWork GetUnitOfWork(IApplicationDbContext dbContext, ILogger<UnitOfWork> logger)
        {
            var unitOfWork = new UnitOfWork(dbContext, logger);

            return unitOfWork;
        }
    }
}
