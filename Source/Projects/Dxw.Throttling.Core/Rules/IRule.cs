namespace Dxw.Throttling.Core.Rules
{
    public interface IRule : INamed { }

    public interface IRule<out T, in TArg>: IRule
    {
        IApplyResult<T> Apply(TArg context = default(TArg));
    }

    //public interface IRule : IRule<object, object> { }
}
