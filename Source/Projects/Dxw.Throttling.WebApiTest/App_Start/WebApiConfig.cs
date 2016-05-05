using System;
using System.Web.Http;

using Dxw.Throttling.Core.Processor;
using Dxw.Throttling.Core.Keyer;
using Dxw.Throttling.Core.Rules;
using Dxw.Throttling.Core.Storage;
using Dxw.Throttling.Asp;

namespace Dxw.Throttling.WebApiTest
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var storage = new LocalMemoryStorage();
            var keyer = new ConstantKeyer();
            //var keyer = new ControllerNameKeyer();
            //var processor = new ConstantEventProcessor() { Ok = true};
            var processor = new RequestCountPerPeriodProcessor() { Count = 1, Period = TimeSpan.FromSeconds(10) };
            var ruleBlock = new StorageKeyerProcessorRule { Storage = storage, Keyer = keyer, Processor = processor };
            var throttlingHandler = new ThrottlingHandler(ruleBlock);
            config.MessageHandlers.Add(throttlingHandler);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
