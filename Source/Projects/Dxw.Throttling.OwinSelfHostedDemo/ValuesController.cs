namespace Dxw.Throttling.OwinSelfHostedDemo
{
    using System.Collections.Generic;
    using System.Web.Http;

    public class ValuesController: ApiController
    {
        public IEnumerable<string> Get()
        {
            return new [] {"val1", "val2"};
        }
    }
}
