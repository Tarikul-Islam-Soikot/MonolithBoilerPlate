using AutoMapper;
using AutoMapper.QueryableExtensions;
using MonolithBoilerPlate.Common;
using MonolithBoilerPlate.Infrastructure.Context;
using MonolithBoilerPlate.Repository.Extensions;
using MonolithBoilerPlate.Repository.Pagination.Interface;
using MonolithBoilerPlate.Repository.Pagination.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MonolithBoilerPlate.Repository.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly IMapper _mapper;
        private readonly ISqlPagingManager _sqlPagingManager;
        readonly DbSet<TEntity> _dbSet;
        public readonly IApplicationDbContext _dbContext;

        public BaseRepository(
            IApplicationDbContext dbContext,
            IMapper mapper,                       
            ISqlPagingManager sqlPagingManager)
        {
            _mapper = mapper;
            _sqlPagingManager = sqlPagingManager;
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Query() => _dbSet;

        public IQueryable<TViewModel> QueryTo<TViewModel>() where TViewModel : class
            => Query().ProjectTo<TViewModel>(_mapper.ConfigurationProvider);

        public virtual async Task<bool> AnyAsync(
                Expression<Func<TEntity, bool>> predicate = null
            )
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate != null)
                return await query.AnyAsync(predicate);
            else
                return await query.AnyAsync();
        }

        #region RAW_SQL

        public async Task<List<T>> RawSqlListAsync<T>(FormattableString sql) where T : class
            => await _dbContext.Set<T>().FromSqlInterpolated(sql).ToListAsync();

        public async Task<T> RawSqlFirstOrDefaultAsync<T>(FormattableString sql) where T : class
            => (await _dbContext.Set<T>().FromSqlInterpolated(sql).ToListAsync()).FirstOrDefault();

        public async Task RawSqlAsync(FormattableString sql)
            => await _dbContext.Database.ExecuteSqlInterpolatedAsync(sql);

        public async Task<List<T>> RawSqlListAsync<T>(string sql, params object[] parameters) where T : class
            => await _dbContext.Set<T>().FromSqlRaw(sql, parameters).ToListAsync();

        public async Task<T> RawSqlFirstOrDefaultAsync<T>(string sql, params object[] parameters) where T : class
            => (await _dbContext.Set<T>().FromSqlRaw(sql, parameters).ToListAsync()).FirstOrDefault();

        public async Task<int> RawSqlAsync(string sql, params object[] parameters)
            => await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);

        #endregion

        #region INSERT_UPDATE_DELETE

        public virtual async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);

            return entity;
        }

        public virtual TEntity Add(TEntity entity)
        {
            _dbSet.Add(entity);

            return entity;
        }

        public virtual async Task InsertRangeAsync(List<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public virtual async Task<List<TEntity>> AddRangeAsync(List<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);

            return entities;
        }

        public virtual List<TEntity> AddRange(List<TEntity> entities)
        {
            _dbSet.AddRange(entities);

            return entities;
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public virtual async Task UpdateRangeAsync(List<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
            await Task.CompletedTask;
        }

        public virtual async Task<TEntity> DeleteAsync(object id)
        {
            var entity = await FirstOrDefaultAsync(id);

            if (entity is null)
                throw new NotFoundException("Entity");

            _dbSet.Remove(entity);

            return entity;
        }


        public virtual async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public virtual async Task DeleteRangeAsync(List<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
            await Task.CompletedTask;
        }

        #endregion

        #region FIRST_OR_DEFAULT

        public virtual async Task<TEntity> FirstOrDefaultAsync(object id)
           => await _dbSet.FindAsync(id);

        public async Task<TEntity> GetEntityAsNoTrackingAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id);
            _dbContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public virtual async Task<TViewModel> FirstOrDefault<TViewModel>(object id) where TViewModel : class
        {
            Expression<Func<TEntity, bool>> lambda = LambdaBuilder.BuildLambdaForFindByKey<TEntity>(id);
            return await Query().Where(lambda).ProjectTo<TViewModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public virtual async Task<TViewModel> FirstOrDefaultToAsync<TViewModel>(
               Expression<Func<TEntity, bool>> predicate = null,
               Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
            )
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).ProjectTo<TViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            else
                return await query.ProjectTo<TViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<TEntity> GetNonTrackingFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
             var entity = await _dbSet.FirstOrDefaultAsync(predicate);
            _dbContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public async Task<TEntity> LastOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy
            )
        {
            return await orderBy(_dbContext.Set<TEntity>())
                .LastOrDefaultAsync(predicate);
        }

        public async Task<TEntity> FirstOrDefaultAsync(
                Expression<Func<TEntity, bool>> predicate,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy
            )
        {
            return await orderBy(_dbContext.Set<TEntity>())
                .FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(
                Expression<Func<TEntity, bool>> predicate,
                params Expression<Func<TEntity, object>>[] includes
            )
        {
            return await includes
            .Aggregate(
                _dbContext.Set<TEntity>().AsQueryable(),
                (current, include) => current.Include(include),
                c => c.AsNoTracking().FirstOrDefaultAsync(predicate)
            ).ConfigureAwait(false);
        }

        public async Task<TEntity> FirstOrDefaultAsync(
                Expression<Func<TEntity, bool>> predicate,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
                params Expression<Func<TEntity, object>>[] includes
            )
        {
            return await includes
            .Aggregate(
                orderBy(_dbContext.Set<TEntity>()).AsQueryable(),
                (current, include) => current.Include(include),
                c => c.FirstOrDefaultAsync(predicate)
            ).ConfigureAwait(false);
        }

        #endregion

        #region SingleOrDeafult

        public virtual async Task<TViewModel> SingleOrDefaultAsync<TViewModel>() where TViewModel : class
        {
            return await Query().ProjectTo<TViewModel>(_mapper.ConfigurationProvider)
                                .SingleOrDefaultAsync();
        }
        #endregion

        #region GET_ALL
        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<List<TViewModel>> GetAllToAsync<TViewModel>(Expression<Func<TEntity, bool>> predicate = null) where TViewModel : class
        {
            if (predicate is not null)
                return await Query().Where(predicate).ProjectTo<TViewModel>(_mapper.ConfigurationProvider).ToListAsync();

            return await Query().ProjectTo<TViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> predicate
            )
        {
            return await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy
            )
        {
            return await orderBy(
                _dbContext.Set<TEntity>().Where(predicate)
            ).ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes
            )
        {
            return await includes.Aggregate(
                _dbContext.Set<TEntity>().AsQueryable(),
                (current, include) => current.Include(include),
                c => c.Where(predicate)
            ).ToListAsync()
            .ConfigureAwait(false);
        }

        public async Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            params Expression<Func<TEntity, object>>[] includes
            )
        {
            return await orderBy(
                includes.Aggregate(
                _dbContext.Set<TEntity>().AsQueryable(),
                (current, include) => current.Include(include),
                c => c.Where(predicate)
            )).ToListAsync()
            .ConfigureAwait(false);
        }

        public virtual async Task<PagedViewModel<TEntity>> GetPagesAsync<IArgs>(
            IQueryable<TEntity> queryable,
            IArgs args
            ) where IArgs : ResourceQueryParameters
        {
            return await _sqlPagingManager.PageAsync(queryable, args);
        }

        public virtual async Task<PagedViewModel<TEntity>> GetPagesAsync<IArgs>(IArgs args)
            where IArgs : ResourceQueryParameters
        {
            return await _sqlPagingManager.PageAsync(Query(), args);
        }

        public virtual async Task<PagedViewModel<TViewModel>> GetPagesToAsync<IArgs, TViewModel>(
            IQueryable<TEntity> queryable, IArgs args)
            where IArgs : ResourceQueryParameters
            where TViewModel : class
        {
            return await _sqlPagingManager.PageAsync<TViewModel, IArgs>(queryable, args);
        }

        public virtual async Task<PagedViewModel<TViewModel>> GetPagesToAsync<IArgs, TViewModel>(IArgs args)
            where IArgs : ResourceQueryParameters
            where TViewModel : class
        {
            return await _sqlPagingManager.PageAsync<TViewModel, IArgs>(Query(), args);
        }

        #endregion

        public virtual void Attach(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Added;
        }

        public virtual void Detach(TEntity entity)
        {
            if (_dbContext.Entry(entity).State != EntityState.Detached)
                _dbContext.Entry(entity).State = EntityState.Detached;
        }
    }
}
