using ChiikinSoft.DistributedLocker;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ChiikinSoft.Tests.DistributedLocker
{
    public class CSRedisLockerTester
    {
        private IServiceProvider serviceProvider;
        [SetUp]
        public void Setup()
        {
            RedisHelper.Initialization(new CSRedis.CSRedisClient("192.168.1.119:6379,defaultDatabase=1"));

            IServiceCollection services = new ServiceCollection();
            services.AddCSRedisLocker();

            serviceProvider= services.BuildServiceProvider();
        }

        [Test]
        public void TestEnter()
        {
            var factory= serviceProvider.GetRequiredService<IDistributedLockerFactory>();
            var locker= factory.Create("TestKey");
            locker.SetExpire(TimeSpan.FromSeconds(60000));
            locker.Enter();
            Thread.Sleep(10000);
            locker.Exit();

            Assert.Pass();
        }
    }
}
