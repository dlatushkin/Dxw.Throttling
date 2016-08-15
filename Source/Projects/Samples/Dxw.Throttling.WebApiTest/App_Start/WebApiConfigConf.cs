namespace Dxw.Throttling.WebApiTest
{
    using System.Configuration;
    using System.Web.Http;

    using Dxw.Throttling.Core.Rules;
    using Dxw.Throttling.Asp;

    public static class WebApiConfigConf
    {
        public static void Register(HttpConfiguration config)
        {
            var throttlingConfig = ConfigurationManager.GetSection("throttling") as Throttling.Core.Configuration.ThrottlingConfiguration<PassBlockVerdict, IAspArgs>;
            var rule = throttlingConfig.Rule;
            var throttlingHandler = new ThrottlingHandler(rule);
            config.MessageHandlers.Add(throttlingHandler);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
