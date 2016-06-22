namespace Dxw.Throttling.Core.Rules
{
    using System;
    using System.Collections.Generic;

    public interface IApplyResultSet
    {
        IEnumerable<IApplyResult> Results { get; }
    }

    public class ApplyResultSetPassBlock : ApplyResultPassBlock, IRuledResult, IApplyResultSet
    {
        public void SetVerdict(PassBlockVerdict verdict)
        {
            _verdict = verdict;
        }

        public IEnumerable<IApplyResult> Results { get; set; }
    }
}
