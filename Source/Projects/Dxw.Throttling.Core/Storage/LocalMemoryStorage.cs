﻿namespace Dxw.Throttling.Core.Storage
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using Dxw.Throttling.Core.Processor;
    using Rules;
    using Configuration;

    public class LocalMemoryStorage : IStorage, IXmlConfigurable, INamed, IDisposable
    {
        private ConcurrentDictionary<object, IStorageValue> _store = new ConcurrentDictionary<object, IStorageValue>();
        private CancellationTokenSource _cleanUpCancellationTokenSource;
        private Task _cleanupTask;

        public LocalMemoryStorage()
        {
            _cleanUpCancellationTokenSource = new CancellationTokenSource();
            _cleanupTask = Task.Factory.StartNew(CleanupRoutine, _cleanUpCancellationTokenSource.Token, TaskCreationOptions.LongRunning);
        }

        public string Name { get; private set; }

        public IProcessEventResult Upsert(object key, object context, IRule rule, Func<object, IStorageValue, IRule, IProcessEventResult> upsertFunc)
        {
            IProcessEventResult result = null;

            Func<object, IStorageValue> addValueFactory = k =>
            {
                result = upsertFunc(context, null, rule);
                return result.NewState;
            };

            Func<object, IStorageValue, IStorageValue> updateValueFactory = (k, v) =>
            {
                result = upsertFunc(context, v, rule);
                return result.NewState;
            };

            var val = _store.AddOrUpdate(key, addValueFactory, updateValueFactory);

            return result;
        }

        public void Dispose()
        {
            _cleanUpCancellationTokenSource.Cancel();
            _cleanupTask.Wait();
            _cleanUpCancellationTokenSource.Dispose();
            _cleanUpCancellationTokenSource = null;
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
