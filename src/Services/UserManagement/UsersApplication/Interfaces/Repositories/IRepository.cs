using System.Linq.Expressions;
using UsersDomain.Abstractions;

namespace UsersApplication.Interfaces.Repositories
{
    public interface IRepository<TEntity, TId>
        where TEntity : Entity<TId>
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entity);

        TEntity? Get(Expression<Func<TEntity, bool>> filter, string? includeProperties = null, bool tracked = false);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, string? includeProperties = null);

        // Async
        Task AddAsync(TEntity entity, CancellationToken cancellationToken);
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter, string? includeProperties = null, bool tracked = false, CancellationToken cancellationToken = default);
    }
}
