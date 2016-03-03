namespace Dxw.Throttling.OwinSelfHostedDemo
{
    using System.Collections.Generic;
    using System.Web.Http;

    public class FirstController: ApiController
    {
        public IEnumerable<string> Get()
        {
            return new [] {"first 1", "first 2"};
        }
    }
}
