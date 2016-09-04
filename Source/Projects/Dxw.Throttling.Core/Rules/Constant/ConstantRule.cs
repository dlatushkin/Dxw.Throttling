namespace Dxw.Throttling.Core.Rules.Constant
{
    using System.Threading.Tasks;
    using System.Xml;
    using Dxw.Throttling.Core.Configuration;
    using Dxw.Throttling.Core.Logging;

    public class ConstantRule<TArg, TRes> : IRule<TArg, TRes>, IXmlConfigurable
    {
        private ILog _log;

        public ConstantRule() {}

        public ConstantRule(ILog log)
        {
            _log = log;
        }

        public TRes Value { get; set; }

        public virtual string Name { get { return this.GetType().Name; } }

        public IApplyResult<TRes> Apply(TArg context = default(TArg))
        {
            var result = new ApplyResult<TRes>
            {
                Rule = this,
                Verdict = Value,
                Reason = new ApplyError { Message = "Always equals " + Value }
            };

            return result;
        }

        public Task<IApplyResult<TRes>> ApplyAsync(TArg context = default(TArg))
        {
            var result = Apply(context);
            return Task.FromResult(result);
        }

        public virtual void Configure(XmlNode node, IConfiguration context)
        {
            _log = context.Log;
        }
    }
}
