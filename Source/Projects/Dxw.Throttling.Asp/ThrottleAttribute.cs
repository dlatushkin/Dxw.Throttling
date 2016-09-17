namespace Dxw.Throttling.Asp
{
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    using Core.Rules;
    using Core.Configuration;
    using Core.Exceptions;
    using Core.Logging;

    public class ThrottleAttribute : ActionFilterAttribute
    {
        private readonly bool _twoPhased;

        private IRule<IAspArgs, PassBlockVerdict> _rule;

        private ILog _log;

        private static IRule<IAspArgs, PassBlockVerdict> GetRule(string configSectionName = null, string ruleName = null)
        {
            configSectionName = configSectionName ?? Const.DFLT_CONFIG_SECTION_NAME;

            var throttlingConfigSection =
                System.Configuration.ConfigurationManager.GetSection(configSectionName) as ThrottlingConfiguration<IAspArgs, PassBlockVerdict>;

            if (throttlingConfigSection == null)
                throw new ThrottlingException(
                    string.Format(
                        "Neither rule was provided nor configuration section '{0}' was setup in config file.",
                            configSectionName));

            var rule = string.IsNullOrWhiteSpace(ruleName) ? throttlingConfigSection.Rule : throttlingConfigSection.GetRuleByName(ruleName);

            return rule;
        }

        public ThrottleAttribute(
            string configSectionName = Const.DFLT_CONFIG_SECTION_NAME, 
            string ruleName = null, 
            bool twoPhased = false)
        {
            var section = ConfigurationRepository<IAspArgs, PassBlockVerdict>.GetSection(configSectionName);
            _rule = section.GetRuleByName(ruleName);
            _log = section.Log;
            _twoPhased = twoPhased;
        }

        #region obsolete ctors
        //public ThrottleAttribute(): this(false, GetRule()) {}

        //public ThrottleAttribute(IRule<IAspArgs, PassBlockVerdict> rule = null, ILog log = null) : this(false, rule) {}

        //public ThrottleAttribute(string configSectionName, string ruleName = null)
        //    : this(GetRule(configSectionName, ruleName)) {}

        //public ThrottleAttribute(bool twoPhased, string configSectionName, string ruleName = null)
        //    : this(twoPhased, GetRule(configSectionName, ruleName)) {}

        //public ThrottleAttribute(bool twoPhased, IRule<IAspArgs, PassBlockVerdict> rule = null)
        //{
        //    _twoPhased = twoPhased;
        //    _rule = rule;
        //} 
        #endregion

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
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

                    _log.Log(LogLevel.Info, GetType().FullName + ".OnActionExecuting: blocked. " + errorMsg);
                }

                _log.Log(LogLevel.Info, GetType().FullName + ".OnActionExecuting: passed. ");

                base.OnActionExecuting(actionContext);
            }
            catch (System.Exception ex)
            {
                _log.Log(LogLevel.Error, GetType().FullName + ".OnActionExecuting: " + ex.Message);
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
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

                    _log.Log(LogLevel.Info, GetType().FullName + ".OnActionExecuted: blocked. " + errorMsg);
                }

                _log.Log(LogLevel.Info, GetType().FullName + ".OnActionExecuted: passed. ");

                base.OnActionExecuted(actionExecutedContext);
            }
            catch (System.Exception ex)
            {
                _log.Log(LogLevel.Error, GetType().FullName + ".OnActionExecuted: " + ex.Message);
            }
        }
    }
}
