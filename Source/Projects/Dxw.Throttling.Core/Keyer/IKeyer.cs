namespace Dxw.Throttling.Core.Keyer
{
    using System.Net.Http;

    public interface IKeyer
    {
        object GetKey(HttpRequestMessage request);
    }
}
