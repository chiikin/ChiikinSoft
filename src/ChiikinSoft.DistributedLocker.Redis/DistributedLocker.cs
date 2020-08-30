using ChiikinSoft.DistributedLocker;
using System;

namespace ChiikinSoft.DistributedLocker.Redis
{
    public class DistributedLocker : IDistributedLocker
    {
        public DistributedLocker()
        {
            //System.Threading.ManualResetEventSlim
        }

        private bool disposedValue;
        private bool locking = false;

        public void Enter()
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }

        public void SetExpire(TimeSpan expireTime)
        {
            throw new NotImplementedException();
        }

        public bool TryEnter(TimeSpan waitTime)
        {
            throw new NotImplementedException();
        }

        #region IDispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                    if (locking)
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
