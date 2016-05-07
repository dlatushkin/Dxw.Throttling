namespace Dxw.Throttling.Redis.Storages
{
    using System;
    using Core.Processors;
    using Core.Rules;
    using Dxw.Throttling.Core.Storages;
    using StackExchange.Redis;

    public class RedisStorage : IStorage
    {
        public RedisStorage()
        {
            var connectionMultiplexer = ConnectionMultiplexer.Connect("localhost");
            var db = connectionMultiplexer.GetDatabase();
            

        }

        public string Name { get; set; }

        public string ConnectionString { get; set; }

        public IProcessEventResult Upsert(object key, object context, IRule rule, Func<object, IStorage, IStorageValue, IRule, IProcessEventResult> upsertFunc)
        {
            throw new NotImplementedException();
        }

        private void EnsureConnected()
        {

        }

        private void Connect()
        {

        }
    }
}
