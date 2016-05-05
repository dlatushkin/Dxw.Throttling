namespace Dxw.Throttling.Core.Rules
{
    public interface IRule
    {
        IApplyResult Apply(object context = null);
    }
}
