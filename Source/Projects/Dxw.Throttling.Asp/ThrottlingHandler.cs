namespace Dxw.Throttling.Asp
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Core.Rules;

    public class ThrottlingHandler: DelegatingHandler
    {
        private IRule _rule;

        public ThrottlingHandler(IRule rule = null)
        {
            _rule = rule;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var applyResult = _rule.Apply(request);
            if (!(bool)applyResult.Verdict)
            {
                return base.SendAsync(request, cancellationToken);
            }

            var errorMsg = applyResult.Reason.Message;

            var response = request.CreateResponse((System.Net.HttpStatusCode)429, errorMsg);
            return Task.FromResult(response);
        }
    }
}
