namespace Dxw.Throttling.Core.Processors
{
    using System.Collections.Concurrent;

    using Rules;
    using Storages;
    using Exceptions;

    public class RequestCountPerPeriodProcessorPhased : RequestCountPerPeriodProcessor<PassBlockVerdict>
    {
        public override IApplyResult<PassBlockVerdict> Process(object key, object context = null, object storeEndpoint = null)
        {
            var dict = storeEndpoint as ConcurrentDictionary<object, IStorageValue<object>>;

            if (dict == null)
                throw new ThrottlingException(
                    "Storage must return valid " + typeof(ConcurrentDictionary<object, IStorageValue<SlotData>>).Name + " store point.");

            IStorageValue<object> newVal = null;

            var phasedContext = context as IPhased;

            if (phasedContext != null && phasedContext.Phase == EventPhase.After)
                newVal = dict.GetOrAdd(key, AddFunc);
            else
                newVal = dict.AddOrUpdate(key, AddFunc, UpdateFunc);

            var newData = newVal.Value as SlotData;

            if (newData.Hits > Count)
                return ApplyResultPassBlock.Block(msg: "The query limit is exceeded.");
            else
                return ApplyResultPassBlock.Pass();
        }
    }
}
