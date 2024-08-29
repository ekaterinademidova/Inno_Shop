using Microsoft.EntityFrameworkCore;
using UsersApplication.Interfaces.Data;
using UsersApplication.Interfaces.Repositories;

namespace UsersInfrastucture.Repositories
{
    public class UserRepository(IApplicationDbContext dbContext) 
        : Repository<User, UserId>(dbContext), IUserRepository
    {
    }
}
