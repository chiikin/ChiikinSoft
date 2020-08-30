using System;
using System.Collections.Generic;
using System.Text;

namespace ChiikinSoft.DistributedLocker.Extensions
{
    public static class DistributedLockerExtensions
    {
        public static bool TryEnter(this IDistributedLockerFactory factory,string key, out IDistributedLocker locker)
        {
            throw new NotImplementedException();
        }

        public static bool TryEnter(this IDistributedLockerFactory factory, string key, TimeSpan waitTime, out IDistributedLocker locker)
        {
            throw new NotImplementedException();
        }

        public static bool TryEnter(this IDistributedLockerFactory factory, string key, TimeSpan waitTime, TimeSpan expireTime, out IDistributedLocker locker)
        {
            throw new NotImplementedException();
        }
    }
}
