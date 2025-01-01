using System.Linq.Expressions;

namespace MonolithBoilerPlate.Service.Base
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        #region NOT_OVERRIDEABLE
        Task<TEntity> FindAsync(long Id);

        #endregion

        #region OVERRIDEABLE

        Task<TEntity> InsertAsync(TEntity entity);
        Task<List<TEntity>> InsertRangeAsync(List<TEntity> entities);
        Task<TEntity> FirstOrDefaultAsync(long id);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity> UpdateAsync(long id, TEntity entity);
        Task<List<TEntity>> UpdateRangeAsync(List<TEntity> entities);
        Task DeleteRangeAsync(List<TEntity> entities);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(long id);

        #endregion
    }
}
