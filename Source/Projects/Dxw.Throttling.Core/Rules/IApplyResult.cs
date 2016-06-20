namespace Dxw.Throttling.Core.Rules
{
    public interface IApplyResult
    {
        bool Block { get; }
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
            return new ApplyResult { Rule = rule, Block = false };
        }

        public static ApplyResult Error(IRule rule = null, string msg = null)
        {
            return new ApplyResult { Rule = rule, Block = true, Reason = new ApplyError { Message = msg } };
        }

        public bool Block { get; set; }

        public IApplyError Reason { get; set; }

        public IRule Rule { get; set; }
    }
}
