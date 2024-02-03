using Microsoft.Extensions.DependencyInjection;
using Spaghetti.Core.Abstractions.CommonService.CacheService;
using Spaghetti.Core.Implementations.CommonService.CacheService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spaghetti.Core.DI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisCacheService(this IServiceCollection services)
        {
            services.AddSingleton<ICacheService, CacheService>();
            /*services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = config.GetConnectionSetting<CacheServerConfig>(ConfigurationKeys.CacheServer).ConnectionString;
            });*/

            // hardCode cause i'm layzy

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379"; // Replace with your Redis instance's host and port
                options.InstanceName = "MyInstance"; // Optional
            });

            return services;
        }
    }
}
