namespace Dxw.Throttling.Asp
{
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    using Core.Rules;
    using Core.Configuration;
    using Core.Exceptions;

    public class ThrottleAttribute : ActionFilterAttribute
    {
        private readonly bool _twoPhased;

        private IRule<PassBlockVerdict, IAspArgs> _rule;

        private static IRule<PassBlockVerdict, IAspArgs> GetRule(string configSectionName = null, string ruleName = null)
        {
            configSectionName = configSectionName ?? Const.DFLT_CONFIG_SECTION_NAME;

            var throttlingConfigSection =
                System.Configuration.ConfigurationManager.GetSection(configSectionName) as ThrottlingConfiguration<PassBlockVerdict, IAspArgs>;

            if (throttlingConfigSection == null)
                throw new ThrottlingException(
                    string.Format(
                        "Neither rule was provided nor configuration section '{0}' was setup in config file.",
                            configSectionName));

            var rule = string.IsNullOrWhiteSpace(ruleName) ? throttlingConfigSection.Rule : throttlingConfigSection.GetRuleByName(ruleName);

            return rule;
        }

        public ThrottleAttribute(): this(false, GetRule()) {}

        public ThrottleAttribute(IRule<PassBlockVerdict, IAspArgs> rule = null) : this(false, rule) {}

        public ThrottleAttribute(string configSectionName, string ruleName = null)
            : this(GetRule(configSectionName, ruleName)) {}

        public ThrottleAttribute(bool twoPhased, string configSectionName, string ruleName = null)
            : this(twoPhased, GetRule(configSectionName, ruleName)) {}

        public ThrottleAttribute(bool twoPhased, IRule<PassBlockVerdict, IAspArgs> rule = null)
        {
            _twoPhased = twoPhased;
            _rule = rule;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var request = actionContext.Request;

            var args = new AspArgs { Phase = Core.EventPhase.Before, Request = request };

            var applyResult = _rule.Apply(args);

            if (applyResult.Verdict == PassBlockVerdict.Block)
            {
                var errorMsg = applyResult.Reason.Message;

                var response429 = new HttpResponseMessage((System.Net.HttpStatusCode)429)
                {
                    RequestMessage = request,
                    ReasonPhrase = errorMsg
                };

                actionContext.Response = response429;
            }

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (!_twoPhased) return;

            var request = actionExecutedContext.Request;
            var response = actionExecutedContext.Response;

            var args = new AspArgs { Phase = Core.EventPhase.After, Request = request, Response = response };

            var applyResult = _rule.Apply(args);

            if (applyResult.Verdict == PassBlockVerdict.Block)
            {
                var errorMsg = applyResult.Reason.Message;

                var response429 = new HttpResponseMessage((System.Net.HttpStatusCode)429)
                {
                    RequestMessage = request,
                    ReasonPhrase = errorMsg
                };

                actionExecutedContext.Response = response429;
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
