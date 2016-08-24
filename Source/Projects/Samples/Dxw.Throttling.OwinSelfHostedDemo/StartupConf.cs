using System;
using System.Web.Http;

using Owin;

using Dxw.Throttling.Core.Storages;
using Dxw.Throttling.Core.Processors;
using Dxw.Throttling.Core.Rules;
using Dxw.Throttling.Owin;
using Microsoft.Owin;
using Dxw.Throttling.Owin.Keyers;
using System.Configuration;

namespace Dxw.Throttling.OwinSelfHostedDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            var throttlingConfig = ConfigurationManager.GetSection("throttling") 
                as Throttling.Core.Configuration.ThrottlingConfiguration<IOwinArgs, PassBlockVerdict>;
            var rule = throttlingConfig.Rule;

            appBuilder.Use(typeof(ThrottlingPassBlockMiddleware), rule);

            appBuilder.UseWebApi(config);
        }
    }
}
