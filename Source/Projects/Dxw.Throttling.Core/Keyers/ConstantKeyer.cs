namespace Dxw.Throttling.Core.Keyers
{
    using System.Net.Http;

    public class ConstantKeyer : IKeyer
    {
        private readonly object _keyObj = new object();

        public object GetKey(object request)
        {
            return _keyObj;
        }
    }
}
