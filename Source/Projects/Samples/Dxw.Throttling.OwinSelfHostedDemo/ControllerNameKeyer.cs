namespace Dxw.Throttling.OwinSelfHostedDemo
{
    using System.Linq;
    using Microsoft.Owin;
    using Core.Keyers;

    public class ControllerNameKeyer : IKeyer<IOwinRequest>
    {
        public object GetKey(IOwinRequest request)
        {
            var owinRequest = request as IOwinRequest;
            if (owinRequest != null)
            {
                var controller = owinRequest.Path.Value.Split('/').Last();
                return controller;
            }
            return null;
        }
    }
}
