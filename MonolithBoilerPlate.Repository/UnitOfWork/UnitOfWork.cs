using AutoMapper;
using MonolithBoilerPlate.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using MonolithBoilerPlate.Repository.Base;
using System.Data;
using MonolithBoilerPlate.Repository.Pagination.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace MonolithBoilerPlate.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable, IAsyncDisposable
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly ISqlPagingManager _sqlPagingManager;
        private bool _disposed;

        public UnitOfWork(
            IApplicationDbContext dbContext,
            IServiceProvider serviceProvider,
            ISqlPagingManager sqlPagingManager,
            IMapper mapper
            )
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
            _sqlPagingManager = sqlPagingManager;
            _mapper = mapper;
        }

        public T GetRepository<T>() where T : class
        {
            return _serviceProvider.GetService<T>();
        }

        public BaseRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return new BaseRepository<TEntity>(_dbContext, _mapper, _sqlPagingManager);
        }

        public int Complete()
        {
            int rowAffected = _dbContext.SaveChanges();
            return rowAffected;
        }

        public async Task<int> CompleteAsync()
        {
            int rowAffected = await _dbContext.SaveChangesAsync();
            return rowAffected;
        }

        public async Task<IDbContextTransaction> CreateTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync(isolationLevel);
            return transaction;
        }

        public IDbContextTransaction CreateTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var transaction = _dbContext.Database.BeginTransaction(isolationLevel);
            return transaction;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    (_dbContext as IDisposable)?.Dispose();
                }

                _disposed = true;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            Dispose(false);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (!_disposed)
            {
                if (_dbContext is IAsyncDisposable asyncDbContext)
                {
                    await asyncDbContext.DisposeAsync();
                }

                _disposed = true;
            }
        }

    }
}
