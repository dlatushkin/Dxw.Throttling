namespace Dxw.Throttling.Core.Rules
{
    using System;
    using System.Collections.Generic;

    //public interface IApplyResult
    //{
    //    object Verdict { get; }
    //    IApplyError Reason { get; }
    //}

    public interface IApplyResult<out T>
    {
        T Verdict { get; }
        IApplyError Reason { get; }
    }

    //public interface IApplyResult : IApplyResult<object> { }

    public interface IRuledResult
    {
        IRule Rule { get; }
    }

    //public interface IRuledResult : IRuledResult<object, object> { };

    public class ApplyResult<T> : IApplyResult<T>, IRuledResult
    {
        public static ApplyResult<T> FromResultAndRule(IApplyResult<T> src, IRule rule)
        {
            return new ApplyResult<T>
            {
                Verdict = src.Verdict,
                Reason = src.Reason,
                Rule = rule
            };
        }

        public T Verdict { get; set; }

        public IApplyError Reason { get; set; }

        public IRule Rule { get; set; }

        IRule IRuledResult.Rule { get { return this.Rule; } }
    }
    
    public enum PassBlockVerdict { Pass, Block }

    public class ApplyResultPassBlock : IApplyResult<PassBlockVerdict>, IRuledResult
    {
        protected PassBlockVerdict _verdict;

        public static IApplyResult<PassBlockVerdict> Pass(IRule rule = null)
        {
            return new ApplyResultPassBlock { Rule = rule, _verdict = PassBlockVerdict.Pass };
        }

        public static IApplyResult<PassBlockVerdict> Block(IRule rule = null, string msg = null)
        {
            return new ApplyResultPassBlock { Rule = rule, _verdict = PassBlockVerdict.Block, Reason = new ApplyError { Message = msg } };
        }

        public PassBlockVerdict Verdict { get { return _verdict; } }

        public IApplyError Reason { get; set; }

        public IRule Rule { get; set; }
    }
}
