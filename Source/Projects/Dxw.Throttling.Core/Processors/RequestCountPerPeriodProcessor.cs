namespace Dxw.Throttling.Core.Processors
{
    using System;
    using Rules;
    using Storages;
    using System.Xml;
    using Configuration;
    using System.Collections.Concurrent;
    using Exceptions;
    using System.Runtime.CompilerServices;

    public class RequestCountPerPeriodProcessor : IProcessor, IXmlConfigurable
    {
        private const int DFLT_COUNT = 1;
        private static readonly TimeSpan DFLT_PERIOD = TimeSpan.FromSeconds(1);

        protected class SlotData
        {
            public int Hits;
            public DateTime ExpiresAt;
        }

        protected class StorageValue: IStorageValue
        {
            public SlotData SlotData;

            public object Value => SlotData;

            public bool IsExpired(DateTime utcNow)
            {
                return utcNow > SlotData.ExpiresAt;
            }
        }

        public int Count { get; set; }

        public TimeSpan Period { get; set; }

        public RequestCountPerPeriodProcessor()
        {
            Count = DFLT_COUNT;
            Period = DFLT_PERIOD;
        }

        public virtual IProcessEventResult Process(object key, object context = null, object storeEndpoint = null, IRule rule = null)
        {
            var dict = storeEndpoint as ConcurrentDictionary<object, IStorageValue>;

            if (dict == null)
                throw new ThrottlingException(
                    "Storage must return valid " + typeof(ConcurrentDictionary<object, IStorageValue>).Name + " store point.");

            var newVal = dict.AddOrUpdate(key, AddFunc, UpdateFunc);
            var newData = newVal.Value as SlotData;

            if (newData.Hits > Count)
                return new ProcessEventResult { NewState = newVal, Result = ApplyResult.Error(rule, "The query limit is exceeded") };
            else
                return new ProcessEventResult { NewState = newVal, Result = ApplyResult.Ok(rule) };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IStorageValue AddFunc(object context)
        {
            var utcNow = DateTime.UtcNow;
            var newVal = new StorageValue { SlotData = new SlotData { Hits = 1, ExpiresAt = utcNow.Add(Period) } };
            return newVal;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IStorageValue UpdateFunc(object context, IStorageValue curValue)
        {
            var utcNow = DateTime.UtcNow;
            var data = curValue.Value as SlotData;
            data.Hits++;
            data.ExpiresAt = utcNow.Add(Period);
            return curValue;
        }

        public void Configure(XmlNode node, IConfiguration context)
        {
            var countAttr = node.Attributes["count"];
            if (countAttr != null)
            {
                int count;
                if (int.TryParse(countAttr.Value, out count))
                    Count = count;
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
