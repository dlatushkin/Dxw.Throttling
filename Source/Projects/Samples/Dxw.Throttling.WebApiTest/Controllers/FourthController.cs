using System.Web.Http;

using Dxw.Throttling.Asp;

namespace Dxw.Throttling.WebApiTest.Controllers
{
    public class FourthController : ApiController
    {
        [Throttle("throttling", "size", true)]
        public string Get()
        {
            return "fourth.get";
        }
    }
}
