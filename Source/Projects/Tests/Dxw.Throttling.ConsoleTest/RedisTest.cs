using System;
using System.Configuration;
using System.Linq;

using Dxw.Throttling.Core;
using Dxw.Throttling.Core.Configuration;
using Dxw.Throttling.Core.Rules;

namespace Dxw.Throttling.ConsoleTest
{
    public static class RedisTest
    {
        public static void Run()
        {
            var throttlingConfiguration = ConfigurationManager.GetSection("throttling") as ThrottlingConfiguration<PassBlockVerdict, object>;

            var redisRule = throttlingConfiguration.Rules.OfType<INamed>().FirstOrDefault(r => r.Name == "singleRedis") as IRule<PassBlockVerdict, object>;

            var res = redisRule.Apply();

            Console.WriteLine(res.Verdict);
        }
    }
}
