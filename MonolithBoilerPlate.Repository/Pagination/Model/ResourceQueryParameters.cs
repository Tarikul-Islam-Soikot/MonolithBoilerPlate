namespace MonolithBoilerPlate.Repository.Pagination.Model
{
    public class ResourceQueryParameters
    {
        private const int MaxPageSize = 200;
        public int PageNumber { get; set; } = 1;
        public string SearchText { get; set; } = string.Empty;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

    }
}
