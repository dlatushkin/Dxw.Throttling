namespace Dxw.Throttling.Asp
{
    using System.Threading.Tasks;

    using Microsoft.Owin;

    using Core.Rules;

    public class ThrottlingMiddleware : OwinMiddleware
    {
        private IRule _rule;

        public ThrottlingMiddleware(OwinMiddleware next, IRule rule = null) : base(next)
        {
            _rule = rule;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var request = context.Request as OwinRequest;

            var applyResult = _rule.Apply(request);
            if (!applyResult.Block)
            {
                await Next.Invoke(context);
                return;
            }

            var response = context.Response;
            var errorMsg = applyResult.Reason.Message;
            response.OnSendingHeaders(state => 
            {
                var resp = (OwinResponse)state;
                //resp.Headers.Add("Retry-After");
                resp.StatusCode = 429;
                resp.ReasonPhrase = errorMsg;
            }, response);
        }
    }
}
