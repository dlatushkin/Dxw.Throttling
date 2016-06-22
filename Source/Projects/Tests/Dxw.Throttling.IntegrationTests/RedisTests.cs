namespace Dxw.Throttling.IntegrationTests
{
    using System;
    using System.Linq;
    using System.Configuration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using Core;
    using Core.Configuration;
    using Core.Rules;

    /// <summary>
    /// Requires connection to redis server.
    /// </summary>
    [TestClass]
    public class RedisTests
    {
        [TestMethod]
        public void Test_01()
        {
            var throttlingConfiguration = ConfigurationManager.GetSection("throttling") as ThrottlingConfiguration;

            var redisRule = throttlingConfiguration.Rules.OfType<INamed>().FirstOrDefault(r => r.Name == "singleRedis") as IRule;

            var res = redisRule.Apply();

            Console.WriteLine(res.GetVerdict<bool>());
        }
    }
}
