namespace Dxw.Throttling.Core.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Dxw.Throttling.Core.Keyer;
    using Dxw.Throttling.Core.Storage;

    public class RuleOrNode : RuleSet
    {
        public override IApplyResult Apply(object context = null, IStorage storage = null)
        {
            var applyResultSet = new ApplyResultSet
            {
                Rule = this,
                Results = new List<IApplyResult>()
            };

            var childResults = new List<IApplyResult>();
            foreach (var rule in Rules)
            {
                var result = rule.Apply(context, storage);

                if (result.Block || !BlockResultsOnly) childResults.Add(result);

                if (!result.Block && !CallEachRule) break;
            }
            applyResultSet.Results = childResults;

            if (childResults.Any(chr => !chr.Block))
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
