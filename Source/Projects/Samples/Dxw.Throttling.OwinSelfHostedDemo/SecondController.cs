namespace Dxw.Throttling.OwinSelfHostedDemo
{
    using System.Collections.Generic;
    using System.Web.Http;

    public class SecondController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new [] {"second 1", "second 2"};
        }
    }
}
