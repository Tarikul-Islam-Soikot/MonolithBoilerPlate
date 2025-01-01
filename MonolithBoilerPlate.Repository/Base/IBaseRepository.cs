using System.Linq.Expressions;

namespace MonolithBoilerPlate.Repository.Base
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Query();
        IQueryable<TViewModel> QueryTo<TViewModel>() where TViewModel : class;
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null);

        #region RawSQL

        Task<List<T>> RawSqlListAsync<T>(FormattableString sql) where T : class;

        Task<T> RawSqlFirstOrDefaultAsync<T>(FormattableString sql) where T : class;

        Task RawSqlAsync(FormattableString sql);

        Task<List<T>> RawSqlListAsync<T>(string sql, params object[] parameters) where T : class;

        Task<T> RawSqlFirstOrDefaultAsync<T>(string sql, params object[] parameters) where T : class;

        Task<int> RawSqlAsync(string sql, params object[] parameters);

        #endregion

        #region INSERT_UPDATE_DELETE
        Task InsertAsync(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
        TEntity Add(TEntity entity);
        Task InsertRangeAsync(List<TEntity> entities);
        Task<List<TEntity>> AddRangeAsync(List<TEntity> entities);
        List<TEntity> AddRange(List<TEntity> entities);

        Task UpdateAsync(TEntity entity);

        Task UpdateRangeAsync(List<TEntity> entities);

        Task<TEntity> DeleteAsync(object id);

        Task DeleteRangeAsync(List<TEntity> entities);

        Task DeleteAsync(TEntity entity);
        #endregion

        #region FirstOrDefault

        Task<TEntity> FirstOrDefaultAsync(object id);
        Task<TEntity> GetEntityAsNoTrackingAsync(object id);

        Task<TViewModel> FirstOrDefault<TViewModel>(object id) where TViewModel : class;

        Task<TEntity> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate
        );

        Task<TEntity> GetNonTrackingFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> LastOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy
        );

        Task<TEntity> FirstOrDefaultAsync(
           Expression<Func<TEntity, bool>> predicate,
           params Expression<Func<TEntity, object>>[] includes
       );

        Task<TEntity> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy
        );

        Task<TEntity> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            params Expression<Func<TEntity, object>>[] includes
        );

        //TODO: multiple include acceptable
        Task<TViewModel> FirstOrDefaultToAsync<TViewModel>(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
         );
        #endregion

        #region SingleOrDeafult
        Task<TViewModel> SingleOrDefaultAsync<TViewModel>() where TViewModel : class;
        #endregion

        #region GET_ALL

        Task<List<TEntity>> GetAllAsync();

        Task<List<TViewModel>> GetAllToAsync<TViewModel>(Expression<Func<TEntity, bool>> predicate = null) where TViewModel : class;

        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] include
        );

        Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy
        );

        Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            params Expression<Func<TEntity, object>>[] includes
        );

        #endregion

        void Attach(TEntity entity);
        void Detach(TEntity entity);

    }
}
