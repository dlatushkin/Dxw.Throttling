namespace Dxw.Throttling.WebApiTest
{
    using System;
    using System.Web.Http;

    using Dxw.Throttling.Core.Rules;
    using Dxw.Throttling.Core.Storages;
    using Dxw.Throttling.Asp;
    using Dxw.Throttling.Asp.Keyers;
    using Core.Processors;

    public static class WebApiConfigCode
    {
        public static void Register(HttpConfiguration config)
        {
            var storage = new LocalMemoryStorage();
            var keyer = new ControllerNameKeyer();
            var processor = new RequestCountPerPeriodProcessorPhased { Count = 1, Period = TimeSpan.FromSeconds(10) };
            var ruleBlock = new StorageKeyerProcessorRule<PassBlockVerdict, IAspArgs> { Storage = storage, Keyer = keyer, Processor = processor } as IRule;
            var throttlingHandler = new ThrottlingHandler(ruleBlock);
            config.MessageHandlers.Add(throttlingHandler);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
