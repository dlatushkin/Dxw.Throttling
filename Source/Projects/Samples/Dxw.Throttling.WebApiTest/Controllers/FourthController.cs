using Dxw.Throttling.Asp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dxw.Throttling.WebApiTest.Controllers
{
    public class FourthController : ApiController
    {
        [Throttle(true, "throttling", "size")]
        public string Get()
        {
            return "fourth.get";
        }
    }
}
