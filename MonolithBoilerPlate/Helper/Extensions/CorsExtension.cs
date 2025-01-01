using MonolithBoilerPlate.Common;
using Microsoft.Extensions.Options;

namespace MonolithBoilerPlate.Api.Helper.Extensions
{
    public static class CorsExtension
    {
        public static void AddCors(this IServiceCollection services, string policyName)
        {
            var appSettings = services.BuildServiceProvider().GetRequiredService<IOptionsSnapshot<AppSettings>>().Value;
            var origins = appSettings.CorsDomain;

            if (origins.Internal.Any())
                origins.External?.AddRange(origins.Internal);

            services.AddCors(options =>
            {
                options.AddPolicy(policyName,
                builder => builder.WithOrigins(origins?.External?.ToArray() ?? Array.Empty<string>())
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });
        }
    }
}
