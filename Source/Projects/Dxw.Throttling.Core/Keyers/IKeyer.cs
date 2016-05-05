namespace Dxw.Throttling.Core.Keyers
{
    public interface IKeyer
    {
        object GetKey(object context);
    }
}
