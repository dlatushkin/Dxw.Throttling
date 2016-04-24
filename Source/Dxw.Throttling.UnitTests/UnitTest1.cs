using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dxw.Throttling.Core.Storage;
using Dxw.Throttling.Core.Keyer;
using Dxw.Throttling.Core.EventProcessor;
using Dxw.Throttling.Core.Rules;

namespace Dxw.Throttling.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test1()
        {
            var storage = new LocalMemoryStorage();
            var keyer = new IPKeyer();
            var processor = new ConstantEventProcessor();

            var rule = new StorageKeyerProcessorRule { Storage = storage, Keyer = keyer, Processor = processor };

            //var httpRequest = new HttpRequestMessage();
            //var context = new RequestContext(RequestPhase.Before, null, )

            //rule.Apply()
        }
    }
}
