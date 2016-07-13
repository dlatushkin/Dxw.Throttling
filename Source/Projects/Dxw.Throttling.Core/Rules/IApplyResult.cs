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

    public interface IApplyResult : IApplyResult<object> { }

    public interface IRuledResult<out T>: IApplyResult<T>
    {
        IRule<T> Rule { get; }
    }

    public interface IRuledResult : IRuledResult<object> { };

    public class ApplyResult<T> : IApplyResult<T>, IRuledResult<T>
    {
        public static ApplyResult<T> FromResultAndRule(IApplyResult<T> src, IRule<T> rule)
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

        public IRule<T> Rule { get; set; }
    }

    public enum PassBlockVerdict { Pass, Block }

    public class ApplyResultPassBlock : IApplyResult<PassBlockVerdict>, IRuledResult<PassBlockVerdict>
    {
        protected PassBlockVerdict _verdict;

        public static IApplyResult<PassBlockVerdict> Pass(IRule<PassBlockVerdict> rule = null)
        {
            return new ApplyResultPassBlock { Rule = rule, _verdict = PassBlockVerdict.Pass };
        }

        public static IApplyResult<PassBlockVerdict> Block(IRule<PassBlockVerdict> rule = null, string msg = null)
        {
            return new ApplyResultPassBlock { Rule = rule, _verdict = PassBlockVerdict.Block, Reason = new ApplyError { Message = msg } };
        }

        public PassBlockVerdict Verdict
        {
            get
            {
                return _verdict;
            }
        }

        public IApplyError Reason { get; set; }

        public IRule<PassBlockVerdict> Rule { get; set; }
    }
}
