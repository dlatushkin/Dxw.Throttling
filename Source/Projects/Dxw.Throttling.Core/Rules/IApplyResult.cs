namespace Dxw.Throttling.Core.Rules
{
    public interface IApplyResult
    {
        bool Block { get; }
        IApplyError Reason { get; }
    }

    public interface IRuleResult
    {
        IRule Rule { get; }
    }

    public class ApplyResult : IApplyResult, IRuleResult
    {
        public static ApplyResult Ok()
        {
            return new ApplyResult { Block = false };
        }

        public static ApplyResult Error(string msg = null)
        {
            return new ApplyResult { Block = true, Reason = new ApplyError { Message = msg } };
        }

        public bool Block { get; set; }

        public IApplyError Reason { get; set; }

        public IRule Rule { get; set; }
    }
}
