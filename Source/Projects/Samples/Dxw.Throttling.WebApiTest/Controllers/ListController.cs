using System.Web.Http;
using Dxw.Throttling.Asp;

namespace Dxw.Throttling.WebApiTest.Controllers
{
    
    public class ListController : ApiController
    {
        [Throttle("whiteList", "white")]
        public string Get()
        {
            return "white-listed";
        }

        [Throttle("blackList", "black")]
        public string Post()
        {
            return "black-listed";
        }
    }
}
