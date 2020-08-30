using System;
using System.Collections.Generic;
using System.Text;

namespace ChiikinSoft.DistributedLocker
{
    public interface IDistributedLockerFactory
    {
        IDistributedLocker Create(string key);

        IDistributedLocker Create(string key, TimeSpan expireTime);

        //bool TryEnter(string key, out IDistributedLocker locker);

        //bool TryEnter(string key, TimeSpan waitTime, out IDistributedLocker locker);

        //bool TryEnter(string key, TimeSpan waitTime, TimeSpan expireTime, out IDistributedLocker locker);

        void SetExpireTime(TimeSpan timeSpan);
    }
}
