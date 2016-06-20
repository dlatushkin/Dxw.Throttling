namespace Dxw.Throttling.Core.Rules
{
    using System.Collections.Generic;

    public interface IApplyResultSet
    {
        IEnumerable<IApplyResult> Results { get; }
    }

    public class ApplyResultSet : IApplyResult, IRuledResult, IApplyResultSet
    {
        public bool Block { get; set; }

        public IApplyError Reason { get; set; }

        public IRule Rule { get; set; }

        public IEnumerable<IApplyResult> Results { get; set; }
    }
}
