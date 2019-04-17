using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Clinic.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity: class
    {
        TEntity Create(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);

        Task<IEnumerable<TEntity>> GetAsync();

        Task<TEntity> GetAsync(int id);

        Task<TEntity> GetAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetFirstAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);

        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetSingleAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);
    }
}
