using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dxw.Throttling.Core.Storages;
using Dxw.Throttling.Core.Keyers;
using Dxw.Throttling.Core.Processors;
using Dxw.Throttling.Core.Rules;
using System.Net.Http;
using System.Linq;

namespace Dxw.Throttling.UnitTests
{
    [TestClass]
    public class Composite
    {
        [TestMethod]
        [TestCategory("Composite")]
        public void T001_And1()
        {
            var storage = new LocalMemoryStorage();
            var keyer = new ConstantKeyer();
            var processor = new ConstantEventProcessor();

            var blockRule = new StorageKeyerProcessorRule { Storage = storage, Keyer = keyer, Processor = processor };

            var rule = new RuleAndNode { CallEachRule = true, BlockResultsOnly = true, Rules = new [] { blockRule } };

            var context = new HttpRequestMessage();
            {
                var r = rule.Apply(context);
                Assert.IsTrue(r.GetVerdict<PassBlockVerdict>() == PassBlockVerdict.Block);

                var ruleSet = r as IApplyResultSet;
                Assert.IsNotNull(ruleSet);
                Assert.IsNotNull(ruleSet.Results);
                Assert.AreEqual(1, ruleSet.Results.Count());

                var blockRuleResult = ruleSet.Results.Single();
                Assert.IsNotNull(blockRuleResult);
                Assert.IsTrue(r.GetVerdict<PassBlockVerdict>() == PassBlockVerdict.Block);

                var blockRuleResultRuled = blockRuleResult as IRuledResult;
                Assert.IsNotNull(blockRuleResultRuled);
                Assert.AreSame(blockRule, blockRuleResultRuled.Rule);
            }
        }

        [TestMethod]
        [TestCategory("Composite")]
        public void T002_And2_BlockPass()
        {
            var storage = new LocalMemoryStorage();
            var keyer = new ConstantKeyer();
            var processor = new ConstantEventProcessor();

            var ruleBlock = new StorageKeyerProcessorRule { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor() };
            var rulePass = new StorageKeyerProcessorRule { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor { Ok=true } };

            var rule = new RuleAndNode { CallEachRule = true, BlockResultsOnly = true, Rules = new[] { ruleBlock, rulePass } };

            var context = new HttpRequestMessage();
            {
                var r = rule.Apply(context);
                Assert.IsTrue(r.GetVerdict<PassBlockVerdict>() == PassBlockVerdict.Block);

                var ruleSet = r as IApplyResultSet;
                Assert.IsNotNull(ruleSet);
                Assert.IsNotNull(ruleSet.Results);
                Assert.AreEqual(1, ruleSet.Results.Count());

                var blockRuleResult = ruleSet.Results.Single();
                Assert.IsNotNull(blockRuleResult);
                Assert.IsTrue(blockRuleResult.GetVerdict<PassBlockVerdict>() == PassBlockVerdict.Block);

                var blockRuleResultRuled = blockRuleResult as IRuledResult;
                Assert.IsNotNull(blockRuleResultRuled);
                Assert.AreSame(ruleBlock, blockRuleResultRuled.Rule);
            }
        }

        [TestMethod]
        [TestCategory("Composite")]
        public void T003_And2_BlockPass_NoBlockResultsOnly()
        {
            var storage = new LocalMemoryStorage();
            var keyer = new ConstantKeyer();
            var processor = new ConstantEventProcessor();

            var ruleBlock = new StorageKeyerProcessorRule { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor() };
            var rulePass = new StorageKeyerProcessorRule { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor { Ok = true } };

            var rule = new RuleAndNode { CallEachRule = true, BlockResultsOnly = false, Rules = new[] { ruleBlock, rulePass } };

            var context = new HttpRequestMessage();
            {
                var r = rule.Apply(context);
                Assert.IsTrue(r.GetVerdict<PassBlockVerdict>() == PassBlockVerdict.Block);

                var ruleSet = r as IApplyResultSet;
                Assert.IsNotNull(ruleSet);
                Assert.IsNotNull(ruleSet.Results);
                Assert.AreEqual(2, ruleSet.Results.Count());

                var blockRuleResult = ruleSet.Results.First();
                Assert.IsNotNull(blockRuleResult);
                Assert.IsTrue(blockRuleResult.GetVerdict<PassBlockVerdict>() == PassBlockVerdict.Block);

                var blockRuleResultRuled = blockRuleResult as IRuledResult;
                Assert.IsNotNull(blockRuleResultRuled);
                Assert.AreSame(ruleBlock, blockRuleResultRuled.Rule);

                var passRuleResult = ruleSet.Results.Skip(1).First();
                Assert.IsNotNull(passRuleResult);
                Assert.IsFalse(passRuleResult.GetVerdict<PassBlockVerdict>() == PassBlockVerdict.Block);

                var passRuleResultRuled = passRuleResult as IRuledResult;
                Assert.IsNotNull(passRuleResultRuled);
                Assert.AreSame(rulePass, passRuleResultRuled.Rule);
            }
        }

        [TestMethod]
        [TestCategory("Composite")]
        public void T004_And2_BlockPass_CallEachRule()
        {
            var storage = new LocalMemoryStorage();
            var keyer = new ConstantKeyer();
            var processor = new ConstantEventProcessor();

            var ruleBlock = new StorageKeyerProcessorRule { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor() };
            var rulePass = new StorageKeyerProcessorRule { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor { Ok = true } };

            var rule = new RuleAndNode { CallEachRule = false, BlockResultsOnly = false, Rules = new[] { ruleBlock, rulePass } };

            var context = new HttpRequestMessage();
            {
                var r = rule.Apply(context);
                Assert.IsTrue(r.GetVerdict<PassBlockVerdict>() == PassBlockVerdict.Block);

                var ruleSet = r as IApplyResultSet;
                Assert.IsNotNull(ruleSet);
                Assert.IsNotNull(ruleSet.Results);
                Assert.AreEqual(1, ruleSet.Results.Count());

                var blockRuleResult = ruleSet.Results.First();
                Assert.IsNotNull(blockRuleResult);
                Assert.IsTrue(blockRuleResult.GetVerdict<PassBlockVerdict>() == PassBlockVerdict.Block);

                var blockRuleResultRuled = blockRuleResult as IRuledResult;
                Assert.IsNotNull(blockRuleResultRuled);
                Assert.AreSame(ruleBlock, blockRuleResultRuled.Rule);
            }
        }

        [TestMethod]
        [TestCategory("Composite")]
        public void T005_And2_PassPass()
        {
            var storage = new LocalMemoryStorage();
            var keyer = new ConstantKeyer();
            var processor = new ConstantEventProcessor();

            var rulePass1 = new StorageKeyerProcessorRule { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor { Ok = true} };
            var rulePass2 = new StorageKeyerProcessorRule { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor { Ok = true } };

            var rule = new RuleAndNode { CallEachRule = true, BlockResultsOnly = true, Rules = new[] { rulePass1, rulePass2 } };

            var context = new HttpRequestMessage();
            {
                var r = rule.Apply(context);
                Assert.IsFalse(r.GetVerdict<PassBlockVerdict>() == PassBlockVerdict.Block);
            }
        }

        [TestMethod]
        [TestCategory("Composite")]
        public void T006_And2_12_Block()
        {
            var storage = new LocalMemoryStorage();
            var keyer = new ConstantKeyer();
            var processor = new ConstantEventProcessor();

            var ruleBlock1 = new StorageKeyerProcessorRule { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor() };
            var rulePass1 = new StorageKeyerProcessorRule { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor { Ok = true } };
            var rule1 = new RuleAndNode { CallEachRule = true, BlockResultsOnly = true, Rules = new[] { ruleBlock1, rulePass1 } };

            var ruleBlock2 = new StorageKeyerProcessorRule { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor() };
            var rulePass2 = new StorageKeyerProcessorRule { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor { Ok = true } };
            var rule2 = new RuleAndNode { CallEachRule = true, BlockResultsOnly = true, Rules = new[] { ruleBlock2, rulePass2 } };

            var rule = new RuleAndNode { CallEachRule = true, BlockResultsOnly = true, Rules = new[] { rule1, rule2 } };

            var context = new HttpRequestMessage();
            {
                var r = rule.Apply(context);
                Assert.IsTrue(r.GetVerdict<PassBlockVerdict>() == PassBlockVerdict.Block);

                var ruleSet = r as IApplyResultSet;
                Assert.IsNotNull(ruleSet);

                //ruleSet.Results
            }
        }
    }
}
