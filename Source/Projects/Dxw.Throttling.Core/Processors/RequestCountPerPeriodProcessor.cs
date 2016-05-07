namespace Dxw.Throttling.Core.Processors
{
    using System;
    using Rules;
    using Storages;
    using System.Xml;
    using Configuration;

    public class RequestCountPerPeriodProcessor : IProcessor, IXmlConfigurable
    {
        private const int DFLT_COUNT = 1;
        private static readonly TimeSpan DFLT_PERIOD = TimeSpan.FromSeconds(1);

        private class SlotData
        {
            public int Hits;
            public DateTime ExpiresAt;
        }

        private class StorageValue: IStorageValue
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

        public IProcessEventResult Process(object context = null, IStorage storage = null, IStorageValue prevState = null, IRule rule = null)
        {
            var utcNow = DateTime.UtcNow;

            ProcessEventResult result;
            
            StorageValue storageValue = prevState as StorageValue;
            if (storageValue == null || storageValue.SlotData.ExpiresAt < utcNow)
            {
                storageValue = new StorageValue { SlotData = new SlotData { Hits = 1, ExpiresAt = utcNow.Add(Period) } };
            }
            else
            {
                storageValue.SlotData.Hits++;
            }

            if (storageValue.SlotData.Hits > Count)
            {
                result = new ProcessEventResult
                {
                    NewState = storageValue,
                    Result = ApplyResult.Error(rule, "The query limit exceeded.")
                };
            }
            else
            {
                result = new ProcessEventResult
                {
                    NewState = storageValue,
                    Result = ApplyResult.Ok()
                };
            }
            
            return result;
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
