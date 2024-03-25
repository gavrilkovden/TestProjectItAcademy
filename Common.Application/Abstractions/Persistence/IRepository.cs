using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        Task<IEnumerable<TEntity>> GetListAsync(
     int? offset = null,
     int? limit = null,
     Expression<Func<TEntity, bool>> predicate = null,
     Expression<Func<TEntity, object>> orderBy = null,
     bool? descending = null);

        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);

        Task<TEntity?> SingleAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);

        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);

        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<bool> DeleteAsync(TEntity entity);
    }
}
