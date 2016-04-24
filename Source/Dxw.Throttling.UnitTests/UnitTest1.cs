using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dxw.Throttling.Core.Storage;
using Dxw.Throttling.Core.Keyer;
using Dxw.Throttling.Core.EventProcessor;
using Dxw.Throttling.Core.Rules;
using System.Net.Http;

namespace Dxw.Throttling.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void T001_ConstantBlock()
        {
            var storage = new LocalMemoryStorage();
            var keyer = new ConstantKeyer();
            var processor = new ConstantEventProcessor();

            var rule = new StorageKeyerProcessorRule { Storage = storage, Keyer = keyer, Processor = processor };

            var context = new HttpRequestMessage();

            {
                var r = rule.Apply(context);
                Assert.IsTrue(r.Block);
            }

            processor.Ok = true;
            {
                var r = rule.Apply(context);
                Assert.IsFalse(r.Block);
            }
        }
    }
}
