namespace Dxw.Throttling.WebApiTest.Controllers
{
    using System.Web.Http;

    using Dxw.Throttling.Asp;

    public class ThirdController : ApiController
    {
        [Throttle]
        public string Get()
        {
            return "third";
        }
    }
}
