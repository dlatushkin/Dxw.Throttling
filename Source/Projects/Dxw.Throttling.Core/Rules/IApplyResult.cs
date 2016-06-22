namespace Dxw.Throttling.Core.Rules
{
    using System;

    public interface IApplyResult
    {
        T GetVerdict<T>();
        IApplyError Reason { get; }
    }

    public interface IRuledResult
    {
        IRule Rule { get; }
    }

    public enum PassBlockVerdict { Pass, Block }

    public class ApplyResultPassBlock : IApplyResult, IRuledResult
    {
        protected PassBlockVerdict _verdict;

        public static IApplyResult Pass(IRule rule = null)
        {
            return new ApplyResultPassBlock { Rule = rule, _verdict = PassBlockVerdict.Pass };
        }

        public static IApplyResult Block(IRule rule = null, string msg = null)
        {
            return new ApplyResultPassBlock { Rule = rule, _verdict = PassBlockVerdict.Block, Reason = new ApplyError { Message = msg } };
        }

        public T GetVerdict<T>()
        {
            return (T)Convert.ChangeType(_verdict, typeof(T));
        }

        public IApplyError Reason { get; set; }

        public IRule Rule { get; set; }
    }
}
