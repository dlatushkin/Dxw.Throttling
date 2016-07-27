namespace Dxw.Throttling.Core.Rules
{
    using System;
    using System.Collections.Generic;

    public interface IApplyResultSet<out T>
    {
        IEnumerable<IApplyResult<T>> Results { get; }
    }

    public interface IApplyResultSet : IApplyResultSet<object> {}

    public class ApplyResultSetPassBlock : ApplyResultPassBlock, IRuledResult, IApplyResultSet<PassBlockVerdict>
    {
        public void SetVerdict(PassBlockVerdict verdict)
        {
            _verdict = verdict;
        }

        public IEnumerable<IApplyResult<PassBlockVerdict>> Results { get; set; }
    }
}
