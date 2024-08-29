using UsersDomain.Abstractions;

namespace UsersApplication.Interfaces.Data
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        DbSet<OperationToken> OperationTokens { get; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}