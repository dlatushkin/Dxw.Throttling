namespace Dxw.Throttling.WebApiTest
{
    using System.Web.Http;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfigAttribute.Register);

            //GlobalConfiguration.Configure(WebApiConfigCode.Register);

            //GlobalConfiguration.Configure(WebApiConfigConf.Register);
        }
    }
}
