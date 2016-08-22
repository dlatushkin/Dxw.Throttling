namespace Dxw.Throttling.WebApiTest
{
    using System.Configuration;
    using System.Web.Http;

    using Dxw.Throttling.Core.Rules;
    using Dxw.Throttling.Asp;

    public static class WebApiConfigAttribute
    {
        public static void Register(HttpConfiguration config)
        {
            var throttlingConfig = ConfigurationManager.GetSection("throttling") 
                as Core.Configuration.ThrottlingConfiguration<PassBlockVerdict, IAspArgs>;

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
