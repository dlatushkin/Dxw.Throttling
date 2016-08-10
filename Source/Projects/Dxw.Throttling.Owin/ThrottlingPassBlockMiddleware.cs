namespace Dxw.Throttling.Owin
{
    using System.Threading.Tasks;

    using Microsoft.Owin;

    using Core.Rules;

    public class ThrottlingPassBlockMiddleware: ThrottlingMiddleware<PassBlockVerdict>
    {
        public ThrottlingPassBlockMiddleware(OwinMiddleware next, IRule<PassBlockVerdict, OwinRequest> rule = null) 
            : base(next, rule, null) { }

        public ThrottlingPassBlockMiddleware(OwinMiddleware next, IRule<PassBlockVerdict, OwinRequest> rule = null, string configSectionName = null) 
            : base(next) { }

        protected override async Task InvokeCore(IOwinContext context, IRule<PassBlockVerdict, OwinRequest> rule)
        {
            var request = context.Request as OwinRequest;

            var applyResult = rule.Apply(request);
            if (applyResult.Verdict == PassBlockVerdict.Pass)
            {
                await Next.Invoke(context);
                return;
            }

            var response = context.Response;
            var errorMsg = applyResult.Reason.Message;
            response.OnSendingHeaders(state => 
            {
                var resp = (OwinResponse)state;
                //resp.Headers.Add("Retry-After", new[] { "10s" });
                resp.StatusCode = 429;
                resp.ReasonPhrase = errorMsg;
            }, response);
        }
    }
}
