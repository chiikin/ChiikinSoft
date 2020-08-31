using System;
using System.Collections.Generic;
using System.Text;

namespace ChiikinSoft.DistributedLocker.CSRedis
{
    public class CSRedisDistributedLockerFactory : IDistributedLockerFactory
    {
        private readonly CSRedisLockerOptions options;
        public CSRedisDistributedLockerFactory(CSRedisLockerOptions options)
        {
            this.options = options;
            watchDog = new WatchDog();
            defaultExpireTime = options.ExpireTime;//默认过期时间30s
        }

        private TimeSpan defaultExpireTime;
        private WatchDog watchDog;


        public IDistributedLocker Create(string key)
        {
            return Create(key,defaultExpireTime);
        }

        public IDistributedLocker Create(string key, TimeSpan expireTime)
        {
            return new CSRedisDistributedLocker(key, GetRedisKey(key), expireTime, watchDog);
        }

        public void SetExpireTime(TimeSpan expireTime)
        {
            defaultExpireTime = expireTime;
        }

        private string GetRedisKey(string key)
        {
            if (string.IsNullOrEmpty(options.KeyPrefix))
                return key;
            else
                return $"{options.KeyPrefix}:{key}";
        }
    }
}
