using System;
using System.Web.Http;

using Owin;

using Dxw.Throttling.Core.Storages;
using Dxw.Throttling.Core.Processors;
using Dxw.Throttling.Core.Rules;
using Dxw.Throttling.Owin;
using Microsoft.Owin;
using Dxw.Throttling.Owin.Keyers;

namespace Dxw.Throttling.OwinSelfHostedDemo
{
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
            var processor = new RequestCountPerPeriodProcessorBlockPass { Count = 1, Period = TimeSpan.FromSeconds(10) };
            var ruleBlock = new StorageKeyerProcessorRule<PassBlockVerdict, IOwinRequest> { Storage = storage, Keyer = keyer, Processor = processor };

            appBuilder.Use(typeof(ThrottlingMiddleware<PassBlockVerdict>), ruleBlock);

            appBuilder.UseWebApi(config);
        }
    }
}
