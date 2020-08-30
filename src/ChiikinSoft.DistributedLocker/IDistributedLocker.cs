﻿using System;

namespace ChiikinSoft.DistributedLocker
{
    public interface IDistributedLocker:IDisposable
    {
        void Enter();

        bool TryEnter(TimeSpan waitTime);

        void Exit();

        void SetExpire(TimeSpan expireTime);
    }
}
