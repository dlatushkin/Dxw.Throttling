namespace Dxw.Throttling.Redis.Processors
{
    using System.Threading.Tasks;

    using StackExchange.Redis;

    using Core.Exceptions;
    using Core.Rules;
    using Core.Logging;

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
                    "storePoint argument must be a valid instance of StackExchange.Redis.IDatabase.");
            }

            var redisKey = key.ToString();

            if (_luaIncrExpire == null)
                _luaIncrExpire = LuaScript.Prepare(LUA_INCR_EXPIRE);

            var result = _luaIncrExpire.Evaluate(db, new { key = (RedisKey)key.ToString(), expireSec = Period.TotalSeconds });

            var hits = (int)result;

            if (hits > Count)
            {
                Log.Log(LogLevel.Debug, string.Format("{0}.Process blocks key='{1}', result='{2}'", GetType().FullName, key, result));
                return ApplyResultPassBlock.Block(msg: "The query limit is exceeded");
            }
            else
            {
                Log.Log(LogLevel.Debug, string.Format("{0}.Process passes key='{1}', result='{2}'", GetType().FullName, key, result));
                return ApplyResultPassBlock.Pass();
            }
        }

        public override async Task<IApplyResult<PassBlockVerdict>> ProcessAsync(object key = null, object context = null, object storeEndpoint = null)
        {
            var db = storeEndpoint as IDatabase;

            if (db == null)
            {
                throw new ThrottlingException("storePoint argument must be a valid instance of StackExchange.Redis.IDatabase.");
            }

            var redisKey = key.ToString();

            if (_luaIncrExpire == null)
                _luaIncrExpire = LuaScript.Prepare(LUA_INCR_EXPIRE);

            var result = await _luaIncrExpire.EvaluateAsync(db, new { key = (RedisKey)key.ToString(), expireSec = Period.TotalSeconds });

            var hits = (int)result;

            if (hits > Count)
            {
                Log.Log(LogLevel.Debug, string.Format("{0}.Process blocks key='{1}', result='{2}'", GetType().FullName, key, result));
                return ApplyResultPassBlock.Block(msg: "The query limit is exceeded");
            }
            else
            {
                Log.Log(LogLevel.Debug, string.Format("{0}.Process passes key='{1}', result='{2}'", GetType().FullName, key, result));
                return ApplyResultPassBlock.Pass();
            }
        }
    }
}
