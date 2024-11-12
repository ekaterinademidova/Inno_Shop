using System.Linq.Expressions;
using UsersDomain.Abstractions;

namespace UsersApplication.Interfaces.RepositoryContracts
{
    public interface IRepository<TEntity, TId>
        where TEntity : Entity<TId>
    {
        Task AddAsync(TEntity entity, CancellationToken cancellationToken);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entity);

        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter, string? includeProperties = null, bool tracked = false, CancellationToken cancellationToken = default);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, string? includeProperties = null);
        
        Task<long> GetTotalCountAsync(CancellationToken cancellationToken = default);
    }
}
