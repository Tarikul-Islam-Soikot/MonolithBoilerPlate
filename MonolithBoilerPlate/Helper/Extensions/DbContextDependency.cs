using MonolithBoilerPlate.Common;
using MonolithBoilerPlate.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MonolithBoilerPlate.Api.Helper.Extensions
{
    public static class DbContextDependency
    {
        public static void AddDbContextDependencies(this IServiceCollection services)
        {
            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>((provider, options) =>
            {
                var appSettings = services.BuildServiceProvider().GetRequiredService<IOptionsSnapshot<AppSettings>>().Value;
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.LogTo(x => Console.WriteLine(x));
                options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection);
            }, ServiceLifetime.Scoped);

        }

    }
}
