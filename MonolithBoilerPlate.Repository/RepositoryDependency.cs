using MonolithBoilerPlate.Repository.Implementation;
using MonolithBoilerPlate.Repository.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace MonolithBoilerPlate.Repository
{
    public static class RepositoryDependency
    {
        public static void AddRepositoryDependency(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IInvoiceApiRequestLogRepository, InvoiceApiRequestLogRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IGeneralApiRequestLogRepository, GeneralApiRequestLogRepository>();
            services.AddScoped<IPrefixConfigRepository, PrefixConfigRepository>();
        }
    }
}
