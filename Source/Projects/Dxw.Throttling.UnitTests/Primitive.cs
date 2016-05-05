using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dxw.Throttling.Core.Storage;
using Dxw.Throttling.Core.Keyer;
using Dxw.Throttling.Core.Processor;
using Dxw.Throttling.Core.Rules;
using System.Net.Http;

namespace Dxw.Throttling.UnitTests
{
    [TestClass]
    public class Primitive
    {
        [TestMethod]
        [TestCategory("Primitive")]
        public void T001_ConstantProcessor()
        {
            var storage = new LocalMemoryStorage();
            var keyer = new ConstantKeyer();
            var processor = new ConstantEventProcessor();

            var rule = new StorageKeyerProcessorRule { Storage = storage, Keyer = keyer, Processor = processor };

            var context = new HttpRequestMessage();

            {
                var r = rule.Apply(context);
                Assert.IsTrue(r.Block);

                var r1 = r as IRuleResult;
                Assert.AreSame(r1.Rule, rule);
            }

            processor.Ok = true;
            {
                var r = rule.Apply(context);
                Assert.IsFalse(r.Block);

                var r1 = r as IRuleResult;
                Assert.AreSame(r1.Rule, rule);
            }
        }
    }
}
