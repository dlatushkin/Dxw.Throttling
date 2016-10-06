using System.Web.Http;

using Dxw.Throttling.Asp;

namespace Dxw.Throttling.WebApiTest.Controllers
{
    public class AttributeController : ApiController
    {
        [Throttle("attrThrottling", "count")]
        public string Post()
        {
            return GetType().Name + ".POST";
        }

        [Throttle("attrThrottling", "size", true)]
        public string Put()
        {
            return GetType().Name + ".PUT";
        }

        [Throttle("attrThrottling", "sizeAndCount", true)]
        public string Get()
        {
            return GetType().Name + ".GET";
        }
    }
}
