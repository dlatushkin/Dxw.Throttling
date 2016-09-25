﻿namespace Dxw.Throttling.Core.Processors
{
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    using Rules;
    using Storages;
    using Exceptions;
    using Logging;

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
            {
                Log.Log(LogLevel.Debug, string.Format("{0}.Process blocks key='{1}', result='{2}'", GetType().FullName, key, newData.Hits));
                return ApplyResultPassBlock.Block(msg: "The query limit is exceeded.");
            }
            else
            {
                Log.Log(LogLevel.Debug, string.Format("{0}.Process passes key='{1}', result='{2}'", GetType().FullName, key, newData.Hits));
                return ApplyResultPassBlock.Pass();
            }
        }

        public override Task<IApplyResult<PassBlockVerdict>> ProcessAsync(object key = null, object context = null, object storeEndpoint = null)
        {
            var result = Process(key, context, storeEndpoint);
            return Task.FromResult(result);
        }
    }
}

