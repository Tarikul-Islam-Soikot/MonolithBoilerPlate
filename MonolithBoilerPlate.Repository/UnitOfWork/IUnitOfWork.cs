using MonolithBoilerPlate.Repository.Base;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace MonolithBoilerPlate.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        T GetRepository<T>() where T : class;
        BaseRepository<TEntity> Repository<TEntity>() where TEntity : class;
        int Complete();
        Task<int> CompleteAsync();
        Task<IDbContextTransaction> CreateTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        IDbContextTransaction CreateTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}
