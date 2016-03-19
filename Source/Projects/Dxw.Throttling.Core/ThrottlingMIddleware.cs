namespace Dxw.Throttling.Core
{
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using System.Net.Http;

    public class ThrottlingMiddleware : OwinMiddleware
    {
        private ThrottleCore _core;

        public ThrottlingMiddleware(OwinMiddleware next, ThrottleCore core = null) : base(next)
        {
            _core = core;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var request = context.Request as HttpRequestMessage;
            var checkResult = _core.CheckRequest(request);
            if (checkResult == null)
            { 
                await Next.Invoke(context);
            }
            else
            {
                return;
            }
        }
    }
}
