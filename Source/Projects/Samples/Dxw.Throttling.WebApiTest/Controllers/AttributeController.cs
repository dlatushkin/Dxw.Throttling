using System.Web.Http;

using Dxw.Throttling.Asp;

namespace Dxw.Throttling.WebApiTest.Controllers
{
    public class AttributeController : ApiController
    {
        [Throttle("attrThrottling", "size", true)]
        public string Get()
        {
            return GetType().Name + ".GET";
        }

        [Throttle("attrThrottling", "count")]
        public string Post()
        {
            return GetType().Name + ".GET";
        }

        [Throttle("attrThrottling", "count")]
        public string Put()
        {
            return GetType().Name + ".GET";
        }
    }
}
