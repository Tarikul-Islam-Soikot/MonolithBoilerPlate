using MonolithBoilerPlate.Common;
using Microsoft.Extensions.Options;
using System.Threading.RateLimiting;

namespace MonolithBoilerPlate.Api.Helper.Extensions
{
    public static class RateLimiting
    {
        public static void AddRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                var appSettings = services.BuildServiceProvider().GetRequiredService<IOptionsSnapshot<AppSettings>>().Value;

                options.AddPolicy(nameof(appSettings.RateLimit.FixedByIP), httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = appSettings.RateLimit.FixedByIP.PermitLimit,
                            Window = TimeSpan.FromMinutes(appSettings.RateLimit.FixedByIP.PeriodInMinutes)
                        }))
                        .OnRejected = async (context, token) =>
                        {
                            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                            await context.HttpContext.Response.WriteAsync("Too many requests. Please try again after some time... ", cancellationToken: token);
                        };
            });

        }
    }

    
}
