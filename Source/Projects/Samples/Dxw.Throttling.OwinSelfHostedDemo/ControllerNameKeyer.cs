namespace Dxw.Throttling.OwinSelfHostedDemo
{
    using System.Linq;
    using Microsoft.Owin;
    using Core.Keyers;

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
