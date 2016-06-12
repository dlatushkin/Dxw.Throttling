namespace Dxw.Throttling.Redis.Processors
{
    using System;

    using StackExchange.Redis;

    using Core.Processors;
    using Core.Rules;
    using Core.Exceptions;

    public class RequestCountPerPeriodProcessor : Core.Processors.RequestCountPerPeriodProcessor
    {
        private const string LUA_INCR_EXPIRE = @"
                                                local current
                                                current = redis.call('incr', @key)
                                                if tonumber(current) == 1 then
                                                    redis.call('expire', @key, 1)
                                                end
                                                return current";

        private static LuaScript _luaIncrExpire;

        public override IProcessEventResult Process(object key, object context, object storeEndpoint, IRule rule = null)
        {
            var db = storeEndpoint as IDatabase;

            if (db == null)
            {
                throw new ThrottlingException(
                    "storePoint argument mst be a valid instance of StackExchange.Redis.IDatabase.");
            }

            var redisKey = key.ToString();

            if (_luaIncrExpire == null)
                _luaIncrExpire = LuaScript.Prepare(LUA_INCR_EXPIRE);

            var result = _luaIncrExpire.Evaluate(db, new { key = (RedisKey)"mykey" });

            var hits = (int)result;

            var newVal = new StorageValue { SlotData = new SlotData { Hits = hits } };

            if (hits > Count)
                return new ProcessEventResult { NewState = newVal, Result = ApplyResult.Error(rule, "The query limit is exceeded") };
            else
                return new ProcessEventResult { NewState = newVal, Result = ApplyResult.Ok(rule) };
        }
    }
}
