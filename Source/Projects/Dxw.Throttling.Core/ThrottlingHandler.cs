namespace Dxw.Throttling.Core
{
    using Rules;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

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
            if (!applyResult.Block)
            {
                return base.SendAsync(request, cancellationToken);
            }

            var errorMsg = applyResult.Reason.Message;

            var response = request.CreateResponse((System.Net.HttpStatusCode)429, errorMsg);
            //response.Headers.Add("Retry-After", new string[] { retryAfter });
            return Task.FromResult(response);
        }
    }
}
