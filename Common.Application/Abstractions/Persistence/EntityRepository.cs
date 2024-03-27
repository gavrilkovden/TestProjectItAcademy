using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Application
{
    public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EntityRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(int? offset = null, int? limit = null, Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, object>> orderBy = null, bool? descending = null)
        {
            IQueryable<TEntity> queryable = _applicationDbContext.Set<TEntity>().AsQueryable();

            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            if (orderBy != null)
            {
                queryable = descending == true ? queryable.OrderByDescending(orderBy) : queryable.OrderBy(orderBy);
            }

            if (offset.HasValue)
            {
                queryable = queryable.Skip(offset.Value);
            }

            if (limit.HasValue)
            {
                queryable = queryable.Take(limit.Value);
            }

            return await queryable.ToListAsync();
        }

        public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            var set = _applicationDbContext.Set<TEntity>();
            return predicate == null ? await set.SingleOrDefaultAsync(cancellationToken) : await set.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            var set = _applicationDbContext.Set<TEntity>();
            if (predicate != null)
            {
                set = (DbSet<TEntity>)set.Where(predicate);
            }

            return await set.CountAsync();
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var set = _applicationDbContext.Set<TEntity>();
            await set.AddAsync(entity, cancellationToken);
            await _applicationDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var set = _applicationDbContext.Set<TEntity>();
            set.Update(entity);
            await _applicationDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            var set = _applicationDbContext.Set<TEntity>();
            set.Remove(entity);
            return await _applicationDbContext.SaveChangesAsync() > 0;
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            var set = _applicationDbContext.Set<TEntity>();
            return predicate == null ? await set.FirstOrDefaultAsync(cancellationToken) : await set.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            var set = _applicationDbContext.Set<TEntity>();
            return predicate == null ? await set.SingleAsync(cancellationToken) : await set.SingleAsync(predicate, cancellationToken);
        }
    }

}
