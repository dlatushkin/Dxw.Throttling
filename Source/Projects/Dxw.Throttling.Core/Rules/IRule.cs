namespace Dxw.Throttling.Core.Rules
{
    public interface IRule : INamed { }

    public interface IRule<out TRes, in TArg>: IRule
    {
        IApplyResult<TRes> Apply(TArg context = default(TArg));
    }
}
