using CSRedis;
using System;
using System.Diagnostics;
using System.Threading;

namespace ChiikinSoft.DistributedLocker.CSRedis
{
    /// <summary>
    /// 基于redis的分布式锁，注意：线程不安全
    /// </summary>
    public class CSRedisDistributedLocker : IDistributedLocker
    {

        public CSRedisDistributedLocker(string key, TimeSpan expireTime, WatchDog watchDog)
        {
            this.key = key;
            this.expireTime = expireTime;
            this.watchDog = watchDog;
            UUID = Guid.NewGuid().ToString("N");
            lockCount = 0;
        }
        private bool disposedValue;
        private string key;
        private bool locking;
        private int lockCount;
        private readonly string UUID;
        private TimeSpan expireTime;
        private WatchDog watchDog;

        public void Enter()
        {
            if (locking)
            {
                lockCount++;
                return;
            }
            TryEnter(new TimeSpan(long.MaxValue));
        }

        public void Exit()
        {
            if (locking && (--lockCount) <= 0)
            {
                RemoveKey();
                locking = false;
            }
        }

        public void SetExpire(TimeSpan expireTime)
        {
            this.expireTime = expireTime;
        }

        public bool TryEnter(TimeSpan waitTime)
        {
            if (locking)
            {
                Interlocked.Increment(ref lockCount);
                return true;
            }
            Stopwatch executeWatch = new Stopwatch();
            executeWatch.Start();
            bool lockSuccess = false;
            do
            {
                lockSuccess = TrySetKey();
                if (lockSuccess)
                {
                    executeWatch.Stop();
                    
                    if (expireTime.Ticks / 2 - executeWatch.ElapsedTicks > 0)
                    {
                        //执行加锁的的时间必须小于1/2 expireTime
                        watchDog.Add(UUID, key, expireTime, expireTime - executeWatch.Elapsed);
                    }
                    else
                    {
                        RemoveKey();
                        lockSuccess = false;
                    }
                    break;
                }
                else
                {
                    if (executeWatch.ElapsedTicks > waitTime.Ticks)
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }
            } while (true);
            return lockSuccess;
        }

        private bool TrySetKey()
        {
            return RedisHelper.Set(key, UUID, expireTime, RedisExistence.Nx);
        }

        private bool RemoveKey()
        {
            string luaScript = "if redis.call('get',ARGV[1]) == ARGV[2] then return redis.call('del',ARGV[1]) else return 0 end";

            return (long)RedisHelper.Eval(luaScript, key, key, UUID) == 1L;
        }

        #region IDispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                    while (locking)
                    {
                        Exit();
                    }
                }

                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~DistributedLocker()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
