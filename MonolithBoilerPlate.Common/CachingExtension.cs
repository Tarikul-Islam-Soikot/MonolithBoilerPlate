using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace MonolithBoilerPlate.Common
{
    public static class CachingExtension
    {
        public static void AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConfig = configuration.GetSection("AppSettings:Redis").Get<RedisConfiguration>();
            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfig);
        }
    }
}
