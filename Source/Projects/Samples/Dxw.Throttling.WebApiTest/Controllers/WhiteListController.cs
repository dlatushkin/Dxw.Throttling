using System.Web.Http;
using Dxw.Throttling.Asp;

namespace Dxw.Throttling.WebApiTest.Controllers
{
    [Throttle("whiteList", "white")]
    public class WhiteListController : ApiController
    {
        public string Get()
        {
            return "white-listed";
        }
    }
}
