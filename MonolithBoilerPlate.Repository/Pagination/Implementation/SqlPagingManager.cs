using AutoMapper;
using AutoMapper.QueryableExtensions;
using MonolithBoilerPlate.Repository.Pagination.Interface;
using MonolithBoilerPlate.Repository.Pagination.Model;

namespace MonolithBoilerPlate.Repository.Pagination.Implementation
{
    public class SqlPagingManager : ISqlPagingManager
    {
        private readonly ISqlPaginator _paginator;
        private readonly IMapper _mapper;

        public SqlPagingManager(ISqlPaginator paginator, IMapper mapper)
        {
            _paginator = paginator;
            _mapper = mapper;
        }

        public async Task<PagedViewModel<TEntity>>
          PageAsync<TEntity, TArgs>(
            IQueryable<TEntity> source,
            TArgs args,
            string? actionName)
          where TArgs : ResourceQueryParameters
        {
            var data = await PagedList<TEntity>
                .CreateAsync(source, args.PageNumber, args.PageSize);

            var metadata = _paginator
               .GetPaginationMetadata(args, data, actionName);

            return new PagedViewModel<TEntity>
            {
                Data = data,
                MetaData = metadata
            };
        }

        public async Task<PagedViewModel<TViewModel>>
            PageAsync<TViewModel, TArgs>(
            IQueryable source,
            TArgs args,
            string? actionName)
            where TArgs : ResourceQueryParameters
        {
            var data = await PagedList<TViewModel>
                .CreateAsync(source
                                .ProjectTo<TViewModel>(_mapper.ConfigurationProvider),
                args.PageNumber,
                args.PageSize);

            var metadata = _paginator
               .GetPaginationMetadata(args, data, actionName);

            return new PagedViewModel<TViewModel>
            {
                Data = data,
                MetaData = metadata
            };
        }
    }
}
