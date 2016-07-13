namespace Dxw.Throttling.Core.Rules
{
    public interface IRule<out T>
    {
        IApplyResult<T> Apply(object context = null);
    }

    public interface IRule : IRule<object> { }
}
