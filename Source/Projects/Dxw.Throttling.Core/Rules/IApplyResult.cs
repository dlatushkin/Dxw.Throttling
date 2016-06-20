namespace Dxw.Throttling.Core.Rules
{
    public interface IApplyResult
    {
        object Verdict { get; }
        IApplyError Reason { get; }
    }

    public interface IRuledResult
    {
        IRule Rule { get; }
    }

    public class ApplyResult : IApplyResult, IRuledResult
    {
        public static ApplyResult Ok(IRule rule = null)
        {
            return new ApplyResult { Rule = rule, Verdict = false };
        }

        public static ApplyResult Error(IRule rule = null, string msg = null)
        {
            return new ApplyResult { Rule = rule, Verdict = true, Reason = new ApplyError { Message = msg } };
        }

        public object Verdict { get; set; }

        public IApplyError Reason { get; set; }

        public IRule Rule { get; set; }
    }
}
