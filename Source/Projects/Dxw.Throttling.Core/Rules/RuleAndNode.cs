﻿namespace Dxw.Throttling.Core.Rules
{
    using System.Collections.Generic;
    using System.Linq;

    public class RuleAndNode : RuleSet<PassBlockVerdict>
    {
        public override IApplyResult<PassBlockVerdict> Apply(object context = null)
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
    }
}
