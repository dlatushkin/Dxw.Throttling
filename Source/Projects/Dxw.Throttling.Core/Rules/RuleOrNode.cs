namespace Dxw.Throttling.Core.Rules
{
    using System.Collections.Generic;
    using System.Linq;

    public class RuleOrNode : RuleSet
    {
        public override IApplyResult Apply(object context = null)
        {
            var applyResultSet = new ApplyResultSetPassBlock
            {
                Rule = this,
                Results = new List<IApplyResult>()
            };

            var childResults = new List<IApplyResult>();
            foreach (var rule in Rules)
            {
                var result = rule.Apply(context);

                if (result.GetVerdict<PassBlockVerdict>() == PassBlockVerdict.Block || !BlockResultsOnly) childResults.Add(result);

                if (result.GetVerdict<PassBlockVerdict>() == PassBlockVerdict.Pass && !CallEachRule) break;
            }
            applyResultSet.Results = childResults;

            if (childResults.Any(chr => chr.GetVerdict<PassBlockVerdict>() == PassBlockVerdict.Pass))
            {
                applyResultSet.SetVerdict(PassBlockVerdict.Pass);
                return applyResultSet;
            }

            applyResultSet.SetVerdict(PassBlockVerdict.Block);
            var firstBlockResult = childResults.FirstOrDefault(chr => chr.GetVerdict<bool>());
            if (firstBlockResult != null) applyResultSet.Reason = firstBlockResult.Reason;

            return applyResultSet;
        }
    }
}
