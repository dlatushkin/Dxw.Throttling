namespace Dxw.Throttling.WebApiTest
{
    using System;
    using System.Web.Http;

    using Dxw.Throttling.Core.Rules;
    using Dxw.Throttling.Core.Storages;
    using Dxw.Throttling.Asp;
    using Dxw.Throttling.Asp.Keyers;
    using Core.Processors;
    using System.Net.Http;
    using Core.Keyers;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var storage = new LocalMemoryStorage();
            //var keyer = new ConstantKeyer();
            var keyer = new ControllerNameKeyer();
            //var processor = new ConstantEventProcessor() { Ok = true};
            var processor = new RequestCountPerPeriodProcessorBlockPass { Count = 1, Period = TimeSpan.FromSeconds(10) };
            var ruleBlock = new StorageKeyerProcessorRule<PassBlockVerdict, HttpRequestMessage> { Storage = storage, Keyer = keyer, Processor = processor } as IRule;
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
