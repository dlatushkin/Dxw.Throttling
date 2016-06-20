namespace Dxw.Throttling.Core.Rules
{
    using System.Collections.Generic;
    using System.Linq;

    public class RuleOrNode : RuleSet
    {
        public override IApplyResult Apply(object context = null)
        {
            var applyResultSet = new ApplyResultSet
            {
                Rule = this,
                Results = new List<IApplyResult>()
            };

            var childResults = new List<IApplyResult>();
            foreach (var rule in Rules)
            {
                var result = rule.Apply(context);

                if ((bool)result.Verdict || !BlockResultsOnly) childResults.Add(result);

                if (!(bool)result.Verdict && !CallEachRule) break;
            }
            applyResultSet.Results = childResults;

            if (childResults.Any(chr => !(bool)chr.Verdict))
            {
                applyResultSet.Verdict = false;
                return applyResultSet;
            }

            applyResultSet.Verdict = true;
            var firstBlockResult = childResults.FirstOrDefault(chr => (bool)chr.Verdict);
            if (firstBlockResult != null) applyResultSet.Reason = firstBlockResult.Reason;

            return applyResultSet;
        }
    }
}
