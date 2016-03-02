namespace Dxw.Throttling.Core.Keyer
{
    using System.Net.Http;

    public class ConstantKeyer : IKeyer
    {
        private readonly object _keyObj = new object();

        public object GetKey(HttpRequestMessage request)
        {
            return _keyObj;
        }
    }
}
