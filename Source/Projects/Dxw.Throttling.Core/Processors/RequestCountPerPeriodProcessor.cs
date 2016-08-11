namespace Dxw.Throttling.Core.Processors
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml;

    using Rules;
    using Storages;
    using Configuration;

    public abstract class RequestCountPerPeriodProcessor<T> : IProcessor<T>, IXmlConfigurable
    {
        private const int DFLT_COUNT = 1;
        private static readonly TimeSpan DFLT_PERIOD = TimeSpan.FromSeconds(1);

        protected class SlotData
        {
            public int Hits;
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

        public int Count { get; set; }

        public TimeSpan Period { get; set; }

        public RequestCountPerPeriodProcessor()
        {
            Count = DFLT_COUNT;
            Period = DFLT_PERIOD;
        }

        public abstract IApplyResult<T> Process(object key, object context = null, object storeEndpoint = null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected IStorageValue<object> AddFunc(object context)
        {
            var utcNow = DateTime.UtcNow;
            var newVal = new StorageValue { SlotData = new SlotData { Hits = 1, ExpiresAt = utcNow.Add(Period) } };
            return newVal;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected IStorageValue<object> UpdateFunc(object context, IStorageValue<object> curValue)
        {
            var utcNow = DateTime.UtcNow;
            var storageValue = curValue as StorageValue;
            if (storageValue.IsExpired(utcNow))
            {
                return AddFunc(context);
            }
            else
            {
                storageValue.Value.Hits++;
                return storageValue;
            }
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
