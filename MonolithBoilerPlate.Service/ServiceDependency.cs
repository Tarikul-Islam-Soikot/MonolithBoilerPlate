using MonolithBoilerPlate.Service.Implementation;
using MonolithBoilerPlate.Service.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace MonolithBoilerPlate.Service
{
    public static class ServiceDependency
    {
        public static void AddServiceDependency(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IInvoiceApiRequestLogService, InvoiceApiRequestLogService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IServerDirectoryService, ServerDirectoryService>();
            services.AddScoped<ICompanyService, CompanyService>();
        }
    }
}
