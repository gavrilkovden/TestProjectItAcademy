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
        TEntity[] GetList(
            int? offset = null,
            int? limit = null,
            Expression<Func<TEntity, bool>>? predicate = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool? descending = null);
        TEntity? SingleOrDefault(Expression<Func<TEntity, bool>>? predicate = null);
        int Count(Expression<Func<TEntity, bool>>? predicate = null);
        TEntity Add(TEntity book);
        TEntity Update(TEntity book);
        bool Delete(TEntity book);
    }
}
