namespace Dxw.Throttling.Core.Keyers
{
    public interface IKeyer<in TArg>
    {
        object GetKey(TArg context);
    }
}
