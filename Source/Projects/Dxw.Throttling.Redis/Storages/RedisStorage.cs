namespace Dxw.Throttling.Redis.Storages
{
    using System;
    using System.IO;

    using System.Runtime.Serialization.Formatters.Binary;
    using System.Xml;
    using Core;
    using Core.Processors;
    using Core.Rules;
    using Core.Storages;
    using Core.Configuration;
    using StackExchange.Redis;
    using System.Runtime.CompilerServices;

    public class RedisStorage : IStorage, IXmlConfigurable
    {
        private IDatabase _db;

        public RedisStorage() {}

        public string Name { get; set; }

        public string ConnectionString { get; set; }


        
        //public IProcessEventResult Upsert(object key, object context, IRule rule, Func<object, IStorage, IStorageValue, IRule, IProcessEventResult> upsertFunc)
        //{
        //    EnsureConnected();
        //    var redisKey = key.ToString();
        //    var oldRedisValue = _db.StringGet(redisKey);

        //    IStorageValue oldValue;
        //    if (oldRedisValue.IsNull)
        //        oldValue = null;
        //    else
        //        oldValue = (IStorageValue)Deserialize(oldRedisValue.ToString());

        //    var result = upsertFunc(key, this, oldValue, rule);
        //    var newRedisValue = Serialize(result.NewState);
        //    _db.StringSet(redisKey, newRedisValue);
        //    return result;
        //}

        public object GetStorePoint()
        {
            EnsureConnected();
            return _db;
        }

        private string Serialize(object obj)
        {
            var formatter = new BinaryFormatter();
            string result;
            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, obj);
                var bytes = ms.ToArray();
                result = Convert.ToBase64String(bytes);
            }
            return result;
        }

        private object Deserialize(string s)
        {
            var formatter = new BinaryFormatter();
            var bytes = Convert.FromBase64String(s);
            using (var ms = new MemoryStream(bytes))
            {
                var obj = formatter.Deserialize(ms);
                return obj;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureConnected()
        {
            if (_db == null)
            {
                Connect();
            }
        }

        private void Connect()
        {
            var connectionMultiplexer = ConnectionMultiplexer.Connect(ConnectionString);
            _db = connectionMultiplexer.GetDatabase();
        }

        public void Configure(XmlNode node, IConfiguration context)
        {
            Name = node.Attributes["name"].Value;
            ConnectionString = node.Attributes["connectionString"].Value;
        }
    }
}
