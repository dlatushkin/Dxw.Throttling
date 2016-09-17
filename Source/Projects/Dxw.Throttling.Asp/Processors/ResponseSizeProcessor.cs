namespace Dxw.Throttling.Asp.Processors
{
    using System;
    using System.Xml;

    using Core.Processors;
    using Dxw.Throttling.Core;
    using Core.Configuration;
    using Core.Rules;
    using Core.Storages;
    using System.Runtime.CompilerServices;
    using System.Collections.Concurrent;
    using Core.Exceptions;
    using System.Threading.Tasks;
    using Core.Logging;

    public class ResponseSizeProcessor : IProcessor<IAspArgs, PassBlockVerdict>, IXmlConfigurable
    {
        private ILog _log;

        protected class SlotData
        {
            public long Bytes;
            public DateTime ExpiresAt;
        }

        protected class StorageValue : IStorageValue<SlotData>
        {
            public SlotData SlotData;

            public SlotData Value => SlotData;

            public bool IsExpired(DateTime utcNow)
            {
                return utcNow > SlotData.ExpiresAt;
            }
        }

        public long Bytes { get; set; }

        public TimeSpan Period { get; set; }

        public IApplyResult<PassBlockVerdict> Process(object key = null, IAspArgs context = default(IAspArgs), object storeEndpoint = null)
        {
            var dict = storeEndpoint as ConcurrentDictionary<object, IStorageValue<object>>;

            if (dict == null)
                throw new ThrottlingException(
                    "Storage must return valid " + typeof(ConcurrentDictionary<object, IStorageValue<SlotData>>).Name + " store point.");

            IStorageValue<object> newVal = null;

            var utcNow = DateTime.UtcNow;

            if (context.Phase == EventPhase.Before)
            {
                dict.TryGetValue(key, out newVal);
                if (newVal != null && newVal.IsExpired(utcNow))
                {
                    newVal = null;
                }
            }
            else
            {
                context.Response.Content.LoadIntoBufferAsync().Wait();

                var size = context.Response.Content.Headers.ContentLength ?? 0;

                newVal = dict.AddOrUpdate(
                    key, 
                    ctx => 
                    {
                        var v = new StorageValue { SlotData = new SlotData { Bytes = size, ExpiresAt = utcNow.Add(Period) } };
                        return v;
                    }, 
                    (ctx, curVal) =>
                    {
                        var storageValue = curVal as StorageValue;
                        if (storageValue.IsExpired(utcNow))
                        {
                            var v = new StorageValue { SlotData = new SlotData { Bytes = size, ExpiresAt = utcNow.Add(Period) } };
                            return v;
                        }
                        else
                        {
                            storageValue.Value.Bytes += size;
                            return storageValue;
                        }
                    }
                );
            }

            if (newVal != null)
            {
                var newData = newVal.Value as SlotData;

                if (newData.Bytes > Bytes)
                    return ApplyResultPassBlock.Block(msg: "The query response size limit is exceeded");
            }

            return ApplyResultPassBlock.Pass();
        }

        public Task<IApplyResult<PassBlockVerdict>> ProcessAsync(object key = null, IAspArgs context = null, object storeEndpoint = null)
        {
            var result = Process(key, context, storeEndpoint);
            return Task.FromResult(result);
        }

        public void Configure(XmlNode node, IConfiguration context)
        {
            _log = context.Log;
            _log.Log(LogLevel.Debug, string.Format("Configuring processor of type '{0}'", GetType().FullName));

            var bytesAttr = node.Attributes["bytes"];
            if (bytesAttr != null)
            {
                long bytes;
                if (long.TryParse(bytesAttr.Value, out bytes))
                    Bytes = bytes;
            }

            var periodAttr = node.Attributes["period"];
            if (periodAttr != null)
            {
                TimeSpan period;
                if (TimeSpan.TryParse(periodAttr.Value, out period))
                    Period = period;
            }
        }
    }
}
