namespace Dxw.Throttling.OwinSelfHostedDemo
{
    using Core.Storages;
    using Owin;
    using System;
    using System.Web.Http;
    using Core.Processors;
    using Core.Rules;
    using Asp;

    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute("Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            var storage = new LocalMemoryStorage();

            //var keyer = new ConstantKeyer();
            var keyer = new ControllerNameKeyer();
            //var processor = new ConstantEventProcessor() { Ok = true};
            var processor = new RequestCountPerPeriodProcessor() { Count = 1, Period = TimeSpan.FromSeconds(10) };
            var ruleBlock = new StorageKeyerProcessorRule { Storage = storage, Keyer = keyer, Processor = processor };

            appBuilder.Use(typeof(ThrottlingMiddleware), ruleBlock);

            appBuilder.UseWebApi(config);
        }
    }
}
