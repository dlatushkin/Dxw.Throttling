using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Throttling
{
    public class ThrottlingStorageConcurrent : IThrottlerStorage
    {
        private ConcurrentDictionary<ThrottlingSlotKey, ThrottlingSlot> _store = new ConcurrentDictionary<ThrottlingSlotKey, ThrottlingSlot>();
        private CancellationTokenSource _cancellationTokenSource;
        private Task _cleanupTask;

        public ThrottlingStorageConcurrent()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cleanupTask = Task.Factory.StartNew(Cleanup, _cancellationTokenSource.Token, TaskCreationOptions.LongRunning);
        }

        public ThrottlingSlotState Hit(ThrottlingSlotKey key, DateTime utcNow)
        {
            var slot = _store.AddOrUpdate(
                key,
                k => ThrottlingSlot.Hit(key.Quota, utcNow),
                (k, v) => v.Hit(utcNow));

            return slot.State;
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cleanupTask.Wait();
            _cancellationTokenSource.Dispose();
            _store.Clear();
            _store = null;
        }

        private void Cleanup(object obj)
        {
            var cancellationToken = (CancellationToken)obj;
            while (!cancellationToken.IsCancellationRequested)
            {
                var utcNow = DateTime.UtcNow;
                foreach (var kv in _store)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        if (kv.Value.IsExpired(utcNow))
                        {
                            ThrottlingSlot slot;
                            _store.TryRemove(kv.Key, out slot);
                        }
                    }
                }
                Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
            }
        }
    }
}
