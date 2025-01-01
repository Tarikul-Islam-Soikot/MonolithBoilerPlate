using MonolithBoilerPlate.Common;
using MonolithBoilerPlate.Repository.UnitOfWork;
using MonolithBoilerPlate.Infrastructure.Utility;
using MonolithBoilerPlate.Service.Helper;
using MonolithBoilerPlate.Common.Interface;
using MonolithBoilerPlate.Common.Helpers;

namespace MonolithBoilerPlate.Api.Helper.Extensions
{
    public static class ApplicationDependency
    {
        public static void AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICurrentUserInfo, CurrentUserInfo>();
            services.AddSingleton<IMessageBrokerHelper, MessageBrokerHelper>();
        }
    }
}
