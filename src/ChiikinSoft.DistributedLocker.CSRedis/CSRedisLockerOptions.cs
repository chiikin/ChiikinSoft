using System;
using System.Collections.Generic;
using System.Text;

namespace ChiikinSoft.DistributedLocker.CSRedis
{
    public class CSRedisLockerOptions
    {
        public TimeSpan ExpireTime { get; set; }
        public string KeyPrefix { get; set; }
    }
}
