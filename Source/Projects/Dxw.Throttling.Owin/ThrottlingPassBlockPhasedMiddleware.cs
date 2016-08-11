namespace Dxw.Throttling.Owin
{
    using System.Threading.Tasks;

    using Microsoft.Owin;

    using Core.Rules;

    public class ThrottlingPassBlockPhasedMiddleware : ThrottlingMiddleware<PassBlockVerdict>
    {
        public ThrottlingPassBlockPhasedMiddleware(OwinMiddleware next, IRule<PassBlockVerdict, IOwinArgs> rule = null) 
            : base(next, rule, null) { }

        public ThrottlingPassBlockPhasedMiddleware(OwinMiddleware next, IRule<PassBlockVerdict, IOwinArgs> rule = null, string configSectionName = null) 
            : base(next) { }

        protected override async Task InvokeCore(IOwinContext context, IRule<PassBlockVerdict, IOwinArgs> rule)
        {
            var args = new OwinArgs { Phase = Core.EventPhase.Before, OwinContext = context };

            var applyResult = rule.Apply(args);
            if (applyResult.Verdict == PassBlockVerdict.Pass)
            {
                await Next.Invoke(context);

                args.Phase = Core.EventPhase.After;
                applyResult = rule.Apply(args);

                if (applyResult.Verdict == PassBlockVerdict.Pass)
                {
                    return;
                }
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
