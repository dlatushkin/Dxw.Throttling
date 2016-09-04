namespace Dxw.Throttling.Core.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class RuleAndNode<TArg> : RuleSet<TArg, PassBlockVerdict>
    {
        public override IApplyResult<PassBlockVerdict> Apply(TArg context = default(TArg))
        {
            var applyResultSet = new ApplyResultSetPassBlock
            {
                Rule = this,
                Results = new List<IApplyResult<PassBlockVerdict>>()
            };

            var childResults = new List<IApplyResult<PassBlockVerdict>>();
            foreach (var rule in Rules)
            {
                var result = rule.Apply(context);

                if (result.Verdict == PassBlockVerdict.Block || !BlockResultsOnly) childResults.Add(result);

                if (result.Verdict == PassBlockVerdict.Block && !CallEachRule) break;
            }
            applyResultSet.Results = childResults;

            if (childResults.All(chr => chr.Verdict == PassBlockVerdict.Pass))
            {
                applyResultSet.SetVerdict(PassBlockVerdict.Pass);
                return applyResultSet;
            }

            applyResultSet.SetVerdict(PassBlockVerdict.Block);
            var firstBlockResult = childResults.FirstOrDefault(chr => chr.Verdict == PassBlockVerdict.Block);
            if (firstBlockResult != null) applyResultSet.Reason = firstBlockResult.Reason;

            return applyResultSet;
        }

        public override async Task<IApplyResult<PassBlockVerdict>> ApplyAsync(TArg context = default(TArg))
        {
            var applyResultSet = new ApplyResultSetPassBlock
            {
                Rule = this,
                Results = new List<IApplyResult<PassBlockVerdict>>()
            };

            var childResults = new List<IApplyResult<PassBlockVerdict>>();
            foreach (var rule in Rules)
            {
                var result = await rule.ApplyAsync(context);

                if (result.Verdict == PassBlockVerdict.Block || !BlockResultsOnly) childResults.Add(result);

                if (result.Verdict == PassBlockVerdict.Block && !CallEachRule) break;
            }
            applyResultSet.Results = childResults;

            if (childResults.All(chr => chr.Verdict == PassBlockVerdict.Pass))
            {
                applyResultSet.SetVerdict(PassBlockVerdict.Pass);
                return applyResultSet;
            }

            applyResultSet.SetVerdict(PassBlockVerdict.Block);
            var firstBlockResult = childResults.FirstOrDefault(chr => chr.Verdict == PassBlockVerdict.Block);
            if (firstBlockResult != null) applyResultSet.Reason = firstBlockResult.Reason;

            return applyResultSet;
        }
    }
}
