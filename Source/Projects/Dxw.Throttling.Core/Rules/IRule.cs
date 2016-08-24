namespace Dxw.Throttling.Core.Rules
{
    public interface IRule : INamed { }

    public interface IRule<in TArg, out TRes> : IRule
    {
        IApplyResult<TRes> Apply(TArg context = default(TArg));
    }
}
