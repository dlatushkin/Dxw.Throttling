using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dxw.Throttling.Core.Storages;
using Dxw.Throttling.Core.Keyers;
using Dxw.Throttling.Core.Processors;
using Dxw.Throttling.Core.Rules;
using System.Net.Http;
using System.Linq;
using Dxw.Throttling.Core.Rules.Constant;
using System.Configuration;
using Dxw.Throttling.Asp.Configuration;
using Dxw.Throttling.Core.Configuration;
using Dxw.Throttling.Asp;

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
            var processor = new ConstantEventProcessor<PassBlockVerdict>() { Value = PassBlockVerdict.Block };

            var blockRule = new StorageKeyerProcessorRule<object, PassBlockVerdict> { Storage = storage, Keyer = keyer, Processor = processor };

            var rule = new RuleAndNode<object> { CallEachRule = true, BlockResultsOnly = true, Rules = new [] { blockRule } };

            var context = new HttpRequestMessage();
            {
                var r = rule.Apply(context);
                Assert.AreEqual(r.Verdict, PassBlockVerdict.Block);

                var ruleSet = r as IApplyResultSet<PassBlockVerdict>;
                Assert.IsNotNull(ruleSet);
                Assert.IsNotNull(ruleSet.Results);
                Assert.AreEqual(1, ruleSet.Results.Count());

                var blockRuleResult = ruleSet.Results.Single();
                Assert.IsNotNull(blockRuleResult);
                Assert.AreEqual(r.Verdict, PassBlockVerdict.Block);

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
            var processor = new ConstantEventProcessor<PassBlockVerdict>();

            var ruleBlock = new StorageKeyerProcessorRule<object, PassBlockVerdict> { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor<PassBlockVerdict>() { Value = PassBlockVerdict.Block } };
            var rulePass = new StorageKeyerProcessorRule<object, PassBlockVerdict> { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor<PassBlockVerdict> { Value=PassBlockVerdict.Pass } };

            var rule = new RuleAndNode<object> { CallEachRule = true, BlockResultsOnly = true, Rules = new[] { ruleBlock, rulePass } };

            var context = new HttpRequestMessage();
            {
                var r = rule.Apply(context);
                Assert.AreEqual(r.Verdict, PassBlockVerdict.Block);

                var ruleSet = r as IApplyResultSet<PassBlockVerdict>;
                Assert.IsNotNull(ruleSet);
                Assert.IsNotNull(ruleSet.Results);
                Assert.AreEqual(1, ruleSet.Results.Count());

                var blockRuleResult = ruleSet.Results.Single();
                Assert.IsNotNull(blockRuleResult);
                Assert.AreEqual(blockRuleResult.Verdict, PassBlockVerdict.Block);

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
            var processor = new ConstantEventProcessor<PassBlockVerdict>();

            var ruleBlock = new StorageKeyerProcessorRule<object, PassBlockVerdict> { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor<PassBlockVerdict> { Value = PassBlockVerdict.Block } };
            var rulePass = new StorageKeyerProcessorRule<object, PassBlockVerdict> { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor<PassBlockVerdict> { Value = PassBlockVerdict.Pass } };

            var rule = new RuleAndNode<object> { CallEachRule = true, BlockResultsOnly = false, Rules = new[] { ruleBlock, rulePass } };

            var context = new HttpRequestMessage();
            {
                var r = rule.Apply(context);
                Assert.AreEqual(r.Verdict, PassBlockVerdict.Block);

                var ruleSet = r as IApplyResultSet<PassBlockVerdict>;
                Assert.IsNotNull(ruleSet);
                Assert.IsNotNull(ruleSet.Results);
                Assert.AreEqual(2, ruleSet.Results.Count());

                var blockRuleResult = ruleSet.Results.First();
                Assert.IsNotNull(blockRuleResult);
                Assert.AreEqual(blockRuleResult.Verdict, PassBlockVerdict.Block);

                var blockRuleResultRuled = blockRuleResult as IRuledResult;
                Assert.IsNotNull(blockRuleResultRuled);
                Assert.AreSame(ruleBlock, blockRuleResultRuled.Rule);

                var passRuleResult = ruleSet.Results.Skip(1).First();
                Assert.IsNotNull(passRuleResult);
                Assert.AreEqual(passRuleResult.Verdict, PassBlockVerdict.Pass);

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
            var processor = new ConstantEventProcessor<PassBlockVerdict>();

            var ruleBlock = new StorageKeyerProcessorRule<object, PassBlockVerdict> { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor<PassBlockVerdict> { Value = PassBlockVerdict.Block } };
            var rulePass = new StorageKeyerProcessorRule<object, PassBlockVerdict> { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor<PassBlockVerdict> { Value = PassBlockVerdict.Pass } };

            var rule = new RuleAndNode<object> { CallEachRule = false, BlockResultsOnly = false, Rules = new[] { ruleBlock, rulePass } };

            var context = new HttpRequestMessage();
            {
                var r = rule.Apply(context);
                Assert.AreEqual(r.Verdict, PassBlockVerdict.Block);

                var ruleSet = r as IApplyResultSet<PassBlockVerdict>;
                Assert.IsNotNull(ruleSet);
                Assert.IsNotNull(ruleSet.Results);
                Assert.AreEqual(1, ruleSet.Results.Count());

                var blockRuleResult = ruleSet.Results.First();
                Assert.IsNotNull(blockRuleResult);
                Assert.AreEqual(r.Verdict, PassBlockVerdict.Block);

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
            var processor = new ConstantEventProcessor<PassBlockVerdict>();

            var rulePass1 = new StorageKeyerProcessorRule<object, PassBlockVerdict> { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor<PassBlockVerdict> { Value = PassBlockVerdict.Pass } };
            var rulePass2 = new StorageKeyerProcessorRule<object, PassBlockVerdict> { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor<PassBlockVerdict> { Value = PassBlockVerdict.Pass } };

            var rule = new RuleAndNode<object> { CallEachRule = true, BlockResultsOnly = true, Rules = new[] { rulePass1, rulePass2 } };

            var context = new HttpRequestMessage();
            {
                var r = rule.Apply(context);
                Assert.AreEqual(r.Verdict, PassBlockVerdict.Pass);
            }
        }

        [TestMethod]
        [TestCategory("Composite")]
        public void T006_And2_12_Block()
        {
            var storage = new LocalMemoryStorage();
            var keyer = new ConstantKeyer();
            var processor = new ConstantEventProcessor<PassBlockVerdict>();

            var ruleBlock1 = new StorageKeyerProcessorRule<object, PassBlockVerdict> { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor<PassBlockVerdict> { Value = PassBlockVerdict.Block} };
            var rulePass1 = new StorageKeyerProcessorRule<object, PassBlockVerdict> { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor<PassBlockVerdict> { Value = PassBlockVerdict.Pass } };
            var rule1 = new RuleAndNode<object> { CallEachRule = true, BlockResultsOnly = true, Rules = new[] { ruleBlock1, rulePass1 } };

            var ruleBlock2 = new StorageKeyerProcessorRule<object, PassBlockVerdict> { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor<PassBlockVerdict> { Value = PassBlockVerdict.Block } };
            var rulePass2 = new StorageKeyerProcessorRule<object, PassBlockVerdict> { Storage = storage, Keyer = keyer, Processor = new ConstantEventProcessor<PassBlockVerdict> { Value = PassBlockVerdict.Pass } };
            var rule2 = new RuleAndNode<object> { CallEachRule = true, BlockResultsOnly = true, Rules = new[] { ruleBlock2, rulePass2 } };

            var rule = new RuleAndNode<object> { CallEachRule = true, BlockResultsOnly = true, Rules = new[] { rule1, rule2 } };

            var context = new HttpRequestMessage();
            {
                var r = rule.Apply(context);
                Assert.AreEqual(r.Verdict, PassBlockVerdict.Block);

                var ruleSet = r as IApplyResultSet<PassBlockVerdict>;
                Assert.IsNotNull(ruleSet);
            }
        }

        [TestMethod]
        public void T010_And()
        {
            var rulePass = new ConstantPassBlockRule<object> { Value = PassBlockVerdict.Pass };
            var ruleBlock = new ConstantPassBlockRule<object> { Value = PassBlockVerdict.Block };

            var andRule = new RuleAndNode<object> { Rules = new[] { rulePass, ruleBlock } };

            var result = andRule.Apply();

            Assert.AreEqual(PassBlockVerdict.Block, result.Verdict);
        }

        [TestMethod]
        public void T011_Or()
        {
            var rulePass = new ConstantPassBlockRule<object> { Value = PassBlockVerdict.Pass };
            var ruleBlock = new ConstantPassBlockRule<object> { Value = PassBlockVerdict.Block };

            var orRule = new RuleOrNode<object> { Rules = new[] { rulePass, ruleBlock } };

            var result = orRule.Apply();

            Assert.AreEqual(PassBlockVerdict.Pass, result.Verdict);
        }

        [TestMethod]
        public void T012_Or()
        {
            var ruleBlock1 = new ConstantPassBlockRule<object> { Value = PassBlockVerdict.Block };
            var ruleBlock2 = new ConstantPassBlockRule<object> { Value = PassBlockVerdict.Block };

            var orRule = new RuleOrNode<object> { Rules = new[] { ruleBlock1, ruleBlock2 } };

            var result = orRule.Apply();

            Assert.AreEqual(PassBlockVerdict.Block, result.Verdict);
        }

        [TestMethod]
        public void T013_AndOr()
        {
            var ruleBlock1 = new ConstantPassBlockRule<object> { Value = PassBlockVerdict.Block };

            var rulePass21 = new ConstantPassBlockRule<object> { Value = PassBlockVerdict.Block };
            var ruleBlock22 = new ConstantPassBlockRule<object> { Value = PassBlockVerdict.Block };

            var ruleOr = new RuleOrNode<object> { Rules = new[] { rulePass21, ruleBlock22 } };

            var ruleAnd = new RuleOrNode<object> { Rules = new IRule<object, PassBlockVerdict>[] { ruleBlock1, ruleOr } };

            var result = ruleAnd.Apply();
             
            Assert.AreEqual(PassBlockVerdict.Block, result.Verdict);
        }

        [TestMethod]
        public void T014_AndConfiguration()
        {
            var config = ConfigurationManager.GetSection(Core.Configuration.Const.DFLT_CONFIG_SECTION_NAME) 
                as ThrottlingConfiguration<IAspArgs, PassBlockVerdict>;

            var result = config.Rule.Apply();
            Assert.AreEqual(PassBlockVerdict.Block, result.Verdict);
        }
    }
}
