namespace Dxw.Throttling.Redis.Processors
{
    using StackExchange.Redis;

    using Core.Processors;
    using Core.Rules;
    using Core.Exceptions;

    public class RequestCountPerPeriodProcessorBlockPass : Core.Processors.RequestCountPerPeriodProcessor<PassBlockVerdict>
    {
        private const string LUA_INCR_EXPIRE = @"
                                                local current
                                                current = redis.call('incr', @key)
                                                if tonumber(current) == 1 then
                                                    redis.call('expire', @key, @expireSec)
                                                end
                                                return current";

        private static LuaScript _luaIncrExpire;

        public override IApplyResult<PassBlockVerdict> Process(object key, object context, object storeEndpoint)
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

            var result = _luaIncrExpire.Evaluate(db, new { key = (RedisKey)key.ToString(), expireSec = Period.TotalSeconds });

            var hits = (int)result;

            var newVal = new StorageValue { SlotData = new SlotData { Hits = hits } };

            if (hits > Count)
                return ApplyResultPassBlock.Block(msg: "The query limit is exceeded");
            else
                return ApplyResultPassBlock.Pass();
        }
    }
}
