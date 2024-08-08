using Microsoft.EntityFrameworkCore;

namespace UsersApplication.Data
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}