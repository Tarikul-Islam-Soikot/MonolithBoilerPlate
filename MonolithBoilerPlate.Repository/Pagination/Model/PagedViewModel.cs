namespace MonolithBoilerPlate.Repository.Pagination.Model
{
    public class PagedViewModel<TViewModel>
    {
        public PagedList<TViewModel> Data { get; set; }
        public PaginationMetadataViewModel MetaData { get; set; }
    }
}
