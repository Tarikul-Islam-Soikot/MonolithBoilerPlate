using MonolithBoilerPlate.Common;
using MonolithBoilerPlate.Service.CronJob;
using MonolithBoilerPlate.Service.Helper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MonolithBoilerPlate.Service
{
    public static class CronJobDependency
    {
        public static void AddCronJobs(this IServiceCollection services)
        {
            var cronJobExpression = services.BuildServiceProvider().GetRequiredService<IOptions<AppSettings>>().Value.CronJobExpression;

            services.AddCronJob<InvoiceSyncWorker>(c =>
            {
                c.CronExpression = cronJobExpression.InvoiceSyncWorker;
            });
        }
    }
}
