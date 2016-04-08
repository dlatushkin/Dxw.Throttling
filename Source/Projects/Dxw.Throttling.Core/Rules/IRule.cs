namespace Dxw.Throttling.Core.Rules
{
    using Dxw.Throttling.Core.Keyer;
    using Storage;

    public interface IRule
    {
        IApplyResult Apply(IRequestContext context = null, IStorage storage = null);
    }
}
