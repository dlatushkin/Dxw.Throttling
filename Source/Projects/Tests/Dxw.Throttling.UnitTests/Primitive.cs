using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dxw.Throttling.Core.Storages;
using Dxw.Throttling.Core.Keyers;
using Dxw.Throttling.Core.Processors;
using Dxw.Throttling.Core.Rules;
using System.Net.Http;
using System;
using System.Collections.Generic;

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
            var processor = new ConstantEventProcessor<PassBlockVerdict>() { Value = PassBlockVerdict.Block };

            var rule = new StorageKeyerProcessorRule<PassBlockVerdict, object> { Storage = storage, Keyer = keyer, Processor = processor };

            var context = new HttpRequestMessage();

            {
                var r = rule.Apply(context);
                Assert.IsTrue(r.Verdict == PassBlockVerdict.Block);

                var r1 = r as IRuledResult;
                Assert.AreSame(r1.Rule, rule);
            }

            processor.Value = PassBlockVerdict.Pass;
            {
                var r = rule.Apply(context);
                Assert.AreEqual(PassBlockVerdict.Pass, r.Verdict);

                var r1 = r as IRuledResult;
                Assert.AreSame(r1.Rule, rule);
            }
        }
    }
}
