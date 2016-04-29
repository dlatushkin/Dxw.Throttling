using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dxw.Throttling.WebApiTest.Controllers
{
    public class FirstController : ApiController
    {
        public string Get()
        {
            return "first";
        }
    }
}
