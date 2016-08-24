namespace Dxw.Throttling.Owin
{
    using System.Threading.Tasks;

    using Microsoft.Owin;

    using Core.Rules;
    using Core.Configuration;
    using Core.Exceptions;

    public abstract class ThrottlingMiddleware<TRes> : OwinMiddleware
    {
        private IRule<IOwinArgs, TRes> _rule;

        private readonly string _configSectionName;

        public ThrottlingMiddleware(OwinMiddleware next) : this(next, null, null) { }

        public ThrottlingMiddleware(OwinMiddleware next, IRule<IOwinArgs, TRes> rule = null) : this(next, rule, null) { }

        public ThrottlingMiddleware(OwinMiddleware next, string configSectionName) : this(next, null, configSectionName) { }

        public ThrottlingMiddleware(OwinMiddleware next, IRule<IOwinArgs, TRes> rule, string configSectionName) : base(next)
        {
            if (rule != null)
            {
                _rule = rule as IRule<IOwinArgs, TRes>;
                return;
            }

            _configSectionName = configSectionName ?? Const.DFLT_CONFIG_SECTION_NAME;

            var throttlingConfigSection = 
                System.Configuration.ConfigurationManager.GetSection(_configSectionName) as ThrottlingConfiguration<IOwinArgs, TRes>;

            if (throttlingConfigSection == null)
                throw new ThrottlingException(
                    string.Format(
                        "Neither rule was provided nor configuration section '{0}' was setup in config file.", 
                            _configSectionName));

            _rule = throttlingConfigSection.Rule as IRule<IOwinArgs, TRes>;
        }

        public override async Task Invoke(IOwinContext context)
        {
            await InvokeCore(context, _rule);
        }

        protected abstract Task InvokeCore(IOwinContext context, IRule<IOwinArgs, TRes> rule);
    }
}
