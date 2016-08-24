namespace Dxw.Throttling.IntegrationTests
{
    using System;
    using System.Linq;
    using System.Configuration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Core;
    using Core.Configuration;
    using Core.Rules;
    using System.Threading;

    /// <summary>
    /// Requires connection to redis server.
    /// </summary>
    [TestClass]
    public class RedisTests
    {
        [TestCategory("Redis")]
        [TestMethod]
        public void RedisTest_01()
        {
            var processort = typeof(Dxw.Throttling.Redis.Processors.RequestCountPerPeriodProcessorBlockPass).FullName;

            var throttlingConfiguration = ConfigurationManager.GetSection("throttling") as ThrottlingConfiguration<object, PassBlockVerdict>;

            var redisRule = throttlingConfiguration.Rules.OfType<INamed>().FirstOrDefault(r => r.Name == "singleRedis") as IRule<object, PassBlockVerdict>;

            IApplyResult<PassBlockVerdict> res;

            res = redisRule.Apply();
            //Assert.AreEqual(res.Verdict, PassBlockVerdict.Pass);

            //res = redisRule.Apply();
            //Assert.AreEqual(res.Verdict, PassBlockVerdict.Pass);

            //res = redisRule.Apply();
            //Assert.AreEqual(res.Verdict, PassBlockVerdict.Block);

            //Thread.Sleep(1001);

            //res = redisRule.Apply();
            //Assert.AreEqual(res.Verdict, PassBlockVerdict.Pass);
        }
    }
}
