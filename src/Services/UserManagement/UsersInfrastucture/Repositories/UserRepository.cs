using Microsoft.EntityFrameworkCore;
using UsersApplication.Interfaces.Data;
using UsersApplication.Interfaces.RepositoryContracts;

namespace UsersInfrastructure.Repositories
{
    public class UserRepository(IApplicationDbContext dbContext) 
        : Repository<User, UserId>(dbContext), IUserRepository
    {
    }
}
