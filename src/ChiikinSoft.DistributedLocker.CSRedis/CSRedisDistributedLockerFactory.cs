using System;
using System.Collections.Generic;
using System.Text;

namespace ChiikinSoft.DistributedLocker.CSRedis
{
    public class CSRedisDistributedLockerFactory : IDistributedLockerFactory
    {
        public IDistributedLocker Create(string key)
        {
            throw new NotImplementedException();
        }

        public IDistributedLocker Create(string key, TimeSpan expireTime)
        {
            throw new NotImplementedException();
        }

        public void SetExpire(TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }

        public void SetWaitTime(TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }
    }
}
