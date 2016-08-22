namespace Dxw.Throttling.Owin
{
    using System.Threading.Tasks;

    using Microsoft.Owin;

    using Core.Rules;
    using Core.Configuration;
    using Core.Exceptions;

    public abstract class ThrottlingMiddleware<T> : OwinMiddleware
    {
        private IRule<T, IOwinArgs> _rule;

        private readonly string _configSectionName;

        public ThrottlingMiddleware(OwinMiddleware next) : this(next, null, null) { }

        public ThrottlingMiddleware(OwinMiddleware next, IRule<T, IOwinArgs> rule = null) : this(next, rule, null) { }

        public ThrottlingMiddleware(OwinMiddleware next, string configSectionName) : this(next, null, configSectionName) { }

        public ThrottlingMiddleware(OwinMiddleware next, IRule<T, IOwinArgs> rule, string configSectionName) : base(next)
        {
            if (rule != null)
            {
                _rule = rule as IRule<T, IOwinArgs>;
                return;
            }

            _configSectionName = configSectionName ?? Const.DFLT_CONFIG_SECTION_NAME;

            var throttlingConfigSection = 
                System.Configuration.ConfigurationManager.GetSection(_configSectionName) as ThrottlingConfiguration<T, IOwinArgs>;

            if (throttlingConfigSection == null)
                throw new ThrottlingException(
                    string.Format(
                        "Neither rule was provided nor configuration section '{0}' was setup in config file.", 
                            _configSectionName));

            _rule = throttlingConfigSection.Rule as IRule<T, IOwinArgs>;
        }

        public override async Task Invoke(IOwinContext context)
        {
            await InvokeCore(context, _rule);
        }

        protected abstract Task InvokeCore(IOwinContext context, IRule<T, IOwinArgs> rule);
    }
}
