namespace Dxw.Throttling.Core.Rules
{
    using System.Collections.Generic;
    using System.Linq;

    public class RuleAndNode : RuleSet
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

                if (result.GetVerdict<PassBlockVerdict>() == PassBlockVerdict.Block && !CallEachRule) break;
            }
            applyResultSet.Results = childResults;

            if (childResults.All(chr => chr.GetVerdict<PassBlockVerdict>() == PassBlockVerdict.Pass))
            {
                applyResultSet.SetVerdict(PassBlockVerdict.Pass);
                return applyResultSet;
            }

            applyResultSet.SetVerdict(PassBlockVerdict.Block);
            var firstBlockResult = childResults.FirstOrDefault(chr => chr.GetVerdict<PassBlockVerdict>() == PassBlockVerdict.Block);
            if (firstBlockResult != null) applyResultSet.Reason = firstBlockResult.Reason;

            return applyResultSet;
        }
    }
}
