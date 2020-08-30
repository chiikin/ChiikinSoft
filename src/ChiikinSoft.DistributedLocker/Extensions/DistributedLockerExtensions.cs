using System;
using System.Collections.Generic;
using System.Text;

namespace ChiikinSoft.DistributedLocker.Extensions
{
    public static class DistributedLockerExtensions
    {
        public static bool TryEnter(this IDistributedLockerFactory factory, string key, TimeSpan waitTime, out IDistributedLocker locker)
        {
            locker = factory.Create(key);
            if (locker.TryEnter(waitTime))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TryEnter(this IDistributedLockerFactory factory, string key, TimeSpan waitTime, TimeSpan expireTime, out IDistributedLocker locker)
        {
            locker = factory.Create(key);
            locker.SetExpire(expireTime);
            if (locker.TryEnter(waitTime))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TryEnter(this IDistributedLocker locker, long waitSeconds)
        {
            return locker.TryEnter(TimeSpan.FromSeconds(waitSeconds));
        }
    }
}
