namespace Dxw.Throttling.Owin
{
    using System.Threading.Tasks;

    using Microsoft.Owin;

    using Core.Rules;
    using Core.Configuration;
    using Core.Exceptions;

    public abstract class ThrottlingMiddleware<T> : OwinMiddleware
    {
        private readonly string DFLT_CONFIG_SECTION_NAME = "throttling";

        private IRule<T, OwinRequest> _rule;

        private readonly string _configSectionName;

        public ThrottlingMiddleware(OwinMiddleware next) : this(next, null, null) { }

        public ThrottlingMiddleware(OwinMiddleware next, IRule<T, OwinRequest> rule = null) : this(next, rule, null) { }

        public ThrottlingMiddleware(OwinMiddleware next, string configSectionName) : this(next, null, configSectionName) { }

        public ThrottlingMiddleware(OwinMiddleware next, IRule<T, OwinRequest> rule, string configSectionName) : base(next)
        {
            if (rule != null)
            {
                _rule = rule as IRule<T, OwinRequest>;
                return;
            }

            _configSectionName = configSectionName ?? DFLT_CONFIG_SECTION_NAME;

            var throttlingConfigSection = 
                System.Configuration.ConfigurationManager.GetSection(_configSectionName) as ThrottlingConfiguration<T, OwinRequest>;

            if (throttlingConfigSection == null)
                throw new ThrottlingException(
                    string.Format(
                        "Neither rule was provided nor configuration section '{0}' was setup in config file.", 
                            _configSectionName));

            _rule = throttlingConfigSection.Rule as IRule<T, OwinRequest>;
        }

        public override async Task Invoke(IOwinContext context)
        {
            await InvokeCore(context, _rule);
        }

        protected abstract Task InvokeCore(IOwinContext context, IRule<T, OwinRequest> rule);
    }
}
