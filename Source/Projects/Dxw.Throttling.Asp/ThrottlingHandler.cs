namespace Dxw.Throttling.Asp
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Core.Rules;

    public class ThrottlingHandler: DelegatingHandler
    {
        private IRule<PassBlockVerdict, IAspArgs> _rule;

        public ThrottlingHandler(IRule rule = null)
        {
            _rule = rule as IRule<PassBlockVerdict, IAspArgs>;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var args = new AspArgs { Phase = Core.EventPhase.Before, Request = request };

            var applyResult = _rule.Apply(args);
            if (applyResult.Verdict == PassBlockVerdict.Pass)
            {
                return base.SendAsync(request, cancellationToken);
            }

            var errorMsg = applyResult.Reason.Message;

            var response = request.CreateResponse((System.Net.HttpStatusCode)429, errorMsg);
            return Task.FromResult(response);
        }
    }
}
