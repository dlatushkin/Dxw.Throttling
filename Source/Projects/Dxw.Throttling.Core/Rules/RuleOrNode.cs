﻿namespace Dxw.Throttling.Core.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class RuleOrNode<TArg> : RuleSet<TArg, PassBlockVerdict>
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

                if (result.Verdict == PassBlockVerdict.Pass && !CallEachRule) break;
            }
            applyResultSet.Results = childResults;

            if (childResults.Any() && childResults.All(chr => chr.Verdict == PassBlockVerdict.Block))
            {
                applyResultSet.SetVerdict(PassBlockVerdict.Block);
                var firstBlockResult = childResults.FirstOrDefault(chr => chr.Verdict == PassBlockVerdict.Block);
                if (firstBlockResult != null) applyResultSet.Reason = firstBlockResult.Reason;
            }
            else
            {
                applyResultSet.SetVerdict(PassBlockVerdict.Pass);
            }

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

                if (result.Verdict == PassBlockVerdict.Pass && !CallEachRule) break;
            }
            applyResultSet.Results = childResults;

            if (childResults.Any() && childResults.All(chr => chr.Verdict == PassBlockVerdict.Block))
            {
                applyResultSet.SetVerdict(PassBlockVerdict.Block);
                var firstBlockResult = childResults.FirstOrDefault(chr => chr.Verdict == PassBlockVerdict.Block);
                if (firstBlockResult != null) applyResultSet.Reason = firstBlockResult.Reason;
            }
            else
            {
                applyResultSet.SetVerdict(PassBlockVerdict.Pass);
            }

            return applyResultSet;
        }
    }
}
