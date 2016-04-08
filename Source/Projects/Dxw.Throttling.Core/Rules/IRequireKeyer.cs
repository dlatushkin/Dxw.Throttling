namespace Dxw.Throttling.Core.Rules
{
    using Keyer;

    public interface IRequireKeyer
    {
        IKeyer Keyer { set; }
    }
}
