namespace Dxw.Throttling.OwinSelfHostedDemo
{
    using Owin;
    using System;
    using System.Web.Http;

    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute("Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            appBuilder.UseWebApi(config);


        }
    }
}
