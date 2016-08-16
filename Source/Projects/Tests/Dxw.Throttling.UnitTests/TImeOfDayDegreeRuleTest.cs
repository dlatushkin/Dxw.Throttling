using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Dxw.Throttling.UnitTests.Rules;
using Dxw.Throttling.Core.Rules;

namespace Dxw.Throttling.UnitTests
{
    [TestClass]
    public class TimeOfDayDegreeRuleTest
    {
        [TestMethod]
        public void Test01()
        {
            var rule = new TimeOfDayDegreeRule();

            IApplyResult<byte> result;

            result = rule.Apply(new DateTime(2016, 8, 16, 2, 0, 0));
            Assert.AreEqual(1, result.Verdict);

            result = rule.Apply(new DateTime(2016, 8, 16, 7, 0, 0));
            Assert.AreEqual(2, result.Verdict);

            result = rule.Apply(new DateTime(2016, 8, 16, 14, 0, 0));
            Assert.AreEqual(3, result.Verdict);
        }
    }
}
