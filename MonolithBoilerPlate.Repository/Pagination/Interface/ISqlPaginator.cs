using MonolithBoilerPlate.Repository.Pagination.Model;

namespace MonolithBoilerPlate.Repository.Pagination.Interface
{
    public interface ISqlPaginator
    {
        PaginationMetadataViewModel GetPaginationMetadata<TParameter, TPagedListViewModel>(
            TParameter args,
            PagedList<TPagedListViewModel> items,
            string? actioName)
            where TParameter : ResourceQueryParameters;
    }

}
