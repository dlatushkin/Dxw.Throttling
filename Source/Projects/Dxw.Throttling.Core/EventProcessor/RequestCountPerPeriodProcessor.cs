namespace Dxw.Throttling.Core.EventProcessor
{
    using System;
    using Keyer;
    using Rules;
    using Storage;

    public class RequestCountPerPeriodProcessor : IEventProcessor
    {
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

        public IProcessEventResult Process(IRequestContext context = null, IStorageValue prevState = null)
        {
            var utcNow = DateTime.UtcNow;

            ProcessEventResult result;
            
            StorageValue storageValue = prevState as StorageValue;
            if (storageValue == null)
            {
                storageValue = new StorageValue { SlotData = new SlotData { Hits = 1, ExpiresAt = utcNow } };
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
                    Result = ApplyResult.Error("Limit of " + Count + "is exceeded. Try ")
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
    }
}
