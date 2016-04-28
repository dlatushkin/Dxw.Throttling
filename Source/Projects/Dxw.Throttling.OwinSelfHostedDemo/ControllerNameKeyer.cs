namespace Dxw.Throttling.Core.Keyer
{
    using Microsoft.Owin;
    using System.Linq;
    using System.Net.Http;

    public class ControllerNameKeyer : IKeyer
    {
        public object GetKey(object request)
        {
            var owinRequest = request as OwinRequest;
            if (owinRequest != null)
            {
                var controller = owinRequest.Path.Value.Split('/').Last();
                return controller;
            }
            return null;
        }
    }
}
