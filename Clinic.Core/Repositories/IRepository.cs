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

        TEntity Remove(TEntity entity);

        Task<IEnumerable<TEntity>> GetAsync();

        Task<TEntity> GetAsync(long id);

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
