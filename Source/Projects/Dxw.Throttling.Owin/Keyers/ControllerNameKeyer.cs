namespace Dxw.Throttling.Owin.Keyers
{
    using System.Linq;
    using Microsoft.Owin;
    using Core.Keyers;

    public class ControllerNameKeyer : IKeyer<IOwinArgs>
    {
        public object GetKey(IOwinArgs owinArgs)
        {
            var controller = owinArgs.OwinContext.Request.Path.Value.Split('/').Last();
            return controller;
        }
    }
}
