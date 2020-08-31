using ChiikinSoft.DistributedLocker;
using ChiikinSoft.DistributedLocker.CSRedis;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCSRedisLocker(this IServiceCollection services)
        {
            return services.AddCSRedisLocker(null);
        }

        public static IServiceCollection AddCSRedisLocker(this IServiceCollection services, Action<CSRedisLockerOptions> action)
        {
            CSRedisLockerOptions options = new CSRedisLockerOptions()
            {
                KeyPrefix = string.Empty,
                ExpireTime = TimeSpan.FromSeconds(30)
            };
            action?.Invoke(options);

            services.AddSingleton(typeof(CSRedisLockerOptions), options);

            services.AddSingleton<IDistributedLockerFactory, CSRedisDistributedLockerFactory>();

            return services;
        }
    }
}
