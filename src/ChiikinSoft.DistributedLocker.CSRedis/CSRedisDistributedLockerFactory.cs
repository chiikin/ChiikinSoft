using System;
using System.Collections.Generic;
using System.Text;

namespace ChiikinSoft.DistributedLocker.CSRedis
{
    public class CSRedisDistributedLockerFactory : IDistributedLockerFactory
    {
        public CSRedisDistributedLockerFactory()
        {
            watchDog = new WatchDog();
            defaultExpireTime = TimeSpan.FromSeconds(30);//默认过期时间30s
        }

        private TimeSpan defaultExpireTime;
        private WatchDog watchDog;

        public IDistributedLocker Create(string key)
        {
            return new CSRedisDistributedLocker(key, defaultExpireTime, watchDog);
        }

        public IDistributedLocker Create(string key, TimeSpan expireTime)
        {
            return new CSRedisDistributedLocker(key, expireTime, watchDog);
        }

        public void SetExpireTime(TimeSpan expireTime)
        {
            defaultExpireTime = expireTime;
        }
    }
}
