using CSRedis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace ChiikinSoft.DistributedLocker.CSRedis
{
    public class WatchDog
    {
        public WatchDog()
        {
            watchs = new ConcurrentDictionary<string, WatchInfo>();
            runningWatcher = new Stopwatch();
            runningWatcher.Start();
        }

        private ConcurrentDictionary<string, WatchInfo> watchs;
        private Thread watchThread;
        private Stopwatch runningWatcher;

        public void Add(string uuid, string key, TimeSpan expireTime, TimeSpan stillExpireTime)
        {
            WatchInfo watchInfo = new WatchInfo()
            {
                UUID = uuid,
                RedisKey = key,
                RenewalInterval = new TimeSpan(expireTime.Ticks / 2),
                ExpireAtTicks = runningWatcher.ElapsedTicks + stillExpireTime.Ticks
            };
            watchs.AddOrUpdate(uuid, watchInfo, (_key, oldValue) => watchInfo);
            StartThread();
        }

        public void Remove(string uuid)
        {
            watchs.TryRemove(uuid, out WatchInfo _);
        }

        private void StartThread()
        {
            if (watchThread == null)
            {
                lock (watchs)
                {
                    if (watchThread == null)
                    {
                        watchThread = new Thread(new ThreadStart(Watching));
                        watchThread.Start();
                    }
                }
            }

        }

        private void Watching()
        {
            Stopwatch freeTimeWatch = new Stopwatch();
            Stopwatch executeWatch = new Stopwatch();
            freeTimeWatch.Start();
            while (true)
            {
                if (watchs.Count > 0)
                {
                    long currentTicks = runningWatcher.ElapsedTicks;
                    var needRenewals = watchs.Where(x => x.Value.NeedRenewal(currentTicks));
                    foreach (var w in needRenewals)
                    {
                        TimeSpan expireAt = TimeSpan.FromSeconds(w.Value.RenewalInterval.TotalSeconds * 2);
                        executeWatch.Reset();
                        if(UpdateExpire(w.Value.RedisKey, w.Value.UUID, (long)expireAt.TotalSeconds))
                        {
                            executeWatch.Stop();
                            w.Value.ExpireAtTicks = runningWatcher.ElapsedTicks + expireAt.Ticks - executeWatch.ElapsedTicks;
                        }
                    }
                    freeTimeWatch.Restart();
                }

                if (freeTimeWatch.ElapsedMilliseconds > 60000)
                {//空闲1分钟后退出线程
                    break;
                }

                Thread.Sleep(100);
            }
            watchThread = null;
        }

        private bool UpdateExpire(string key, string uuid, long expireSeconds)
        {
            string luaScript = "if redis.call('get',ARGV[1]) == ARGV[2] then return redis.call('expire',ARGV[1],ARGV[3]) else return 0 end";

            return (long)RedisHelper.Eval(luaScript, key, key, uuid, expireSeconds) == 1L;
        }

        private class WatchInfo
        {
            public string UUID { get; set; }

            public string RedisKey { get; set; }

            public TimeSpan RenewalInterval { get; set; }
            public long ExpireAtTicks { get; set; }

            public bool NeedRenewal(long currentTicks)
            {
                return (ExpireAtTicks - currentTicks) < RenewalInterval.Ticks;
            }
        }
    }
}
