using System.Linq.Expressions;

namespace TokenGenerator.Application.Interfaces.IRepositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Remove(TEntity entity);

        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        
        void Update(TEntity entity);

        Task SaveChangesAsync();

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity?> GetByIdAsync(Guid id);

        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    }
}