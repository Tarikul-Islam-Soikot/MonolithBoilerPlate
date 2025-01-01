using MonolithBoilerPlate.Repository.Base;
using MonolithBoilerPlate.Repository.UnitOfWork;
using System.Linq.Expressions;

namespace MonolithBoilerPlate.Service.Base
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        private BaseRepository<TEntity> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public BaseService(IUnitOfWork unitOfWork)
        {
            _repository = unitOfWork.Repository<TEntity>();
            _unitOfWork = unitOfWork;
        }

        #region NOT_OVERRIDEABLE

        public Task<TEntity> FindAsync(long Id)
        {
            return _repository.FirstOrDefaultAsync(Id);
        }

        #endregion


        #region OVERRIDEABLEreportvie

        // CREATE
        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            await _repository.InsertAsync(entity);
            await _unitOfWork.CompleteAsync();
            return entity;
        }

        public virtual async Task<List<TEntity>> InsertRangeAsync(List<TEntity> entities)
        {
            await _repository.InsertRangeAsync(entities);
            await _unitOfWork.CompleteAsync();
            return entities;
        }

        // READ
        public virtual async Task<TEntity> FirstOrDefaultAsync(long id)
        {
            return await _repository.FirstOrDefaultAsync(id);
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _repository.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// UPDATE
        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            await _repository.UpdateAsync(entity);
            await _unitOfWork.CompleteAsync();
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(long id, TEntity entity)
        {
            await _repository.UpdateAsync(entity);
            await _unitOfWork.CompleteAsync();
            return entity;
        }

        public virtual async Task<List<TEntity>> UpdateRangeAsync(List<TEntity> entities)
        {
            await _repository.UpdateRangeAsync(entities);
            await _unitOfWork.CompleteAsync();
            return entities;
        }


        /// DELETE
        public virtual async Task DeleteAsync(TEntity entity)
        {
            await _repository.DeleteAsync(entity);
            await _unitOfWork.CompleteAsync();

        }

        public virtual async Task DeleteAsync(long id)
        {
            await _repository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public virtual async Task DeleteRangeAsync(List<TEntity> entities)
        {
            await _repository.DeleteRangeAsync(entities);
            await _unitOfWork.CompleteAsync();
        }

        #endregion
    }
}
