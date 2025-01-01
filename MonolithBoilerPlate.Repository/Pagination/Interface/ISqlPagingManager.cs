using MonolithBoilerPlate.Repository.Pagination.Model;

namespace MonolithBoilerPlate.Repository.Pagination.Interface
{
    public interface ISqlPagingManager
    {
        Task<PagedViewModel<TViewModel>> PageAsync<TViewModel, TArgs>(
            IQueryable source,
            TArgs args,
            string? actionName = null)
            where TArgs : ResourceQueryParameters;

        Task<PagedViewModel<TEntity>> PageAsync<TEntity, TArgs>(
            IQueryable<TEntity> source,
            TArgs args,
            string? actionName =  null)
            where TArgs : ResourceQueryParameters;
    }
}
