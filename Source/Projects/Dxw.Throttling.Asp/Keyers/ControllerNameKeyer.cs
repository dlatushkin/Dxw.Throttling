namespace Dxw.Throttling.Asp.Keyers
{
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http.Routing;
    using System.Web.Http.Controllers;

    using Core.Keyers;

    public class ControllerNameKeyer : IKeyer<IAspArgs>
    {
        public object GetKey(IAspArgs args)
        {
            var request = args.Request;

            var routeData = request.GetRouteData();

            var controllerName = (string)routeData.Values["controller"];

            return controllerName;

            //var attributedRoutesData = request.GetRouteData().GetSubRoutes();
            //var subRouteData = attributedRoutesData.FirstOrDefault();

            //var actions = (ReflectedHttpActionDescriptor[])subRouteData.Route.DataTokens["actions"];
            //var controllerName = actions[0].ControllerDescriptor.ControllerName;

            //return request.GetActionDescriptor().ActionName;
        }
    }
}
