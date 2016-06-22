namespace Dxw.Throttling.Owin
{
    using System.Threading.Tasks;

    using Microsoft.Owin;

    using Core.Rules;
    using Core.Configuration;
    using Core.Exceptions;

    public class ThrottlingMiddleware : OwinMiddleware
    {
        private readonly string DFLT_CONFIG_SECTION_NAME = "throttling";

        private IRule _rule;

        private readonly string _configSectionName;

        public ThrottlingMiddleware(OwinMiddleware next, IRule rule = null, string configSectionName = null) : base(next)
        {
            if (rule != null)
            {
                _rule = rule;
                return;
            }

            _configSectionName = configSectionName ?? DFLT_CONFIG_SECTION_NAME;

            var throttlingConfigSection = 
                System.Configuration.ConfigurationManager.GetSection(_configSectionName) as ThrottlingConfiguration;

            if (throttlingConfigSection == null)
                throw new ThrottlingException(
                    string.Format(
                        "Neither rule was provided nor configuration section '{0}' was setup in config file.", 
                            _configSectionName));

            _rule = throttlingConfigSection.Rule;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var request = context.Request as OwinRequest;

            var applyResult = _rule.Apply(request);
            if (!applyResult.GetVerdict<bool>())
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
