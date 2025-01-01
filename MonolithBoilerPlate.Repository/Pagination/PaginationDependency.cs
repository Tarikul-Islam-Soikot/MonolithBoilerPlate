using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using MonolithBoilerPlate.Repository.Pagination.Interface;
using MonolithBoilerPlate.Repository.Pagination.Implementation;

namespace MonolithBoilerPlate.Repository.Pagination
{
    public static class PaginationDependency
    {
        public static void AddSqlPagination(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
            services.AddScoped<ISqlPaginator, SqlPaginator>();
            services.AddScoped<ISqlPagingManager, SqlPagingManager>();
        }
    }
}
