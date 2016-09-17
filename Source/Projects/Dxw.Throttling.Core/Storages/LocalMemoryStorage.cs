namespace Dxw.Throttling.Core.Storages
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;

    using Configuration;
    using Logging;

    public class LocalMemoryStorage : IStorage, IXmlConfigurable, INamed, IPurgable, IDisposable
    {
        private ILog _log;

        private ConcurrentDictionary<object, IStorageValue<object>> _store = new ConcurrentDictionary<object, IStorageValue<object>>();

        private CancellationTokenSource _cleanupCancellationTokenSource;

        private Task _cleanupTask;

        public LocalMemoryStorage()
        {
            _cleanupCancellationTokenSource = new CancellationTokenSource();
            _cleanupTask = Task.Factory.StartNew(CleanupRoutine, _cleanupCancellationTokenSource.Token, TaskCreationOptions.LongRunning);
        }

        public string Name { get; private set; }

        public object GetStorePoint()
        {
            return _store;
        }

        public void Dispose()
        {
            _cleanupCancellationTokenSource.Cancel();
            _cleanupTask.Wait();
            _cleanupCancellationTokenSource.Dispose();
            _cleanupCancellationTokenSource = null;
            _store.Clear();
            _store = null;
        }

        private async Task CleanupRoutine(object obj)
        {
            var cancellationToken = (CancellationToken)obj;
            while (!cancellationToken.IsCancellationRequested)
            {
                _log.Log(LogLevel.Debug, string.Format("Cleanup storage '{0}' of type '{1}'", Name, GetType().FullName));
                Cleanup(cancellationToken);
                await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
            }
        }

        private void Cleanup(CancellationToken cancellationToken)
        {
            var utcNow = DateTime.UtcNow;
            foreach (var kv in _store)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    if (kv.Value.IsExpired(utcNow))
                    {
                        IStorageValue<object> val;
                        _store.TryRemove(kv.Key, out val);
                    }
                }
            }
        }

        public void Configure(XmlNode node, IConfiguration context)
        {
            _log = context.Log;
            Name = node.Attributes["name"].Value;

            _log.Log(LogLevel.Debug, string.Format("Configuring storage '{0}' of type '{1}'", Name, GetType().FullName));
        }

        public void Purge()
        {
            _store.Clear();
        }
    }
}
