namespace Dxw.Throttling.Core.Storages
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using Processors;
    using Rules;
    using Configuration;

    public class LocalMemoryStorage : IStorage, IXmlConfigurable, INamed, IDisposable
    {
        private ConcurrentDictionary<object, IStorageValue> _store = new ConcurrentDictionary<object, IStorageValue>();
        private CancellationTokenSource _cleanupCancellationTokenSource;
        private Task _cleanupTask;

        public LocalMemoryStorage()
        {
            _cleanupCancellationTokenSource = new CancellationTokenSource();
            _cleanupTask = Task.Factory.StartNew(CleanupRoutine, _cleanupCancellationTokenSource.Token, TaskCreationOptions.LongRunning);
        }

        public string Name { get; private set; }

        public IProcessEventResult Upsert(object key, object context, IRule rule, Func<object, IStorage, IStorageValue, IRule, IProcessEventResult> upsertFunc)
        {
            IProcessEventResult result = null;

            Func<object, IStorageValue> addValueFactory = k =>
            {
                result = upsertFunc(context, this, null, rule);
                return result.NewState;
            };

            Func<object, IStorageValue, IStorageValue> updateValueFactory = (k, v) =>
            {
                result = upsertFunc(context, this, v, rule);
                return result.NewState;
            };

            var val = _store.AddOrUpdate(key, addValueFactory, updateValueFactory);

            return result;
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
                        IStorageValue val;
                        _store.TryRemove(kv.Key, out val);
                    }
                }
            }
        }

        public void Configure(XmlNode node, IConfiguration context)
        {
            Name = node.Attributes["name"].Value;
        }
    }
}
