namespace Dxw.Throttling.Core.Rules
{
    using System.Collections.Generic;
    using System.Linq;

    public class RuleAndNode : RuleSet
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

                if (result.Block || !BlockResultsOnly) childResults.Add(result);

                if (result.Block && !CallEachRule) break;
            }
            applyResultSet.Results = childResults;

            if (childResults.All(chr => !chr.Block))
            {
                applyResultSet.Block = false;
                return applyResultSet;
            }

            applyResultSet.Block = true;
            var firstBlockResult = childResults.FirstOrDefault(chr => chr.Block);
            if (firstBlockResult != null) applyResultSet.Reason = firstBlockResult.Reason;

            return applyResultSet;
        }
    }
}
