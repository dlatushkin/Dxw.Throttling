namespace Dxw.Throttling.Owin.Keyers
{
    using System.Linq;
    using Microsoft.Owin;
    using Core.Keyers;

    public class ControllerNameKeyer : IKeyer<IOwinRequest>
    {
        public object GetKey(IOwinRequest request)
        {
            if (request != null)
            {
                var controller = request.Path.Value.Split('/').Last();
                return controller;
            }
            return null;
        }
    }
}
