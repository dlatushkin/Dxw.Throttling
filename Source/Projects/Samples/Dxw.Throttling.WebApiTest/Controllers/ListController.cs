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
        public string Head()
        {
            return "black-listed";
        }

        [Throttle("whiteList", "white")]
        public string Post()
        {
            return "white-listed";
        }

        [Throttle("blackList", "black")]
        public string Put()
        {
            return "black-listed";
        }
    }
}
