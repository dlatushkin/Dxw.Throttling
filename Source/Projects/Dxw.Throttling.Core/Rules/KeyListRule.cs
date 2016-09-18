namespace Dxw.Throttling.Core.Rules
{
    using System;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Collections.Generic;

    using Configuration;
    using Keyers;
    using Logging;

    public abstract class KeyListRule<TArg, TRes> : IRule<TArg, TRes>, IXmlConfigurable<TArg, TRes>
    {
        private List<object> _keys = new List<object>();

        protected bool HasKey(object key)
        {
            return _keys.Contains(key);
        }

        public IKeyer<TArg> Keyer { get; set; }

        private ILog _log;

        public string Name { get; private set; }

        public abstract IApplyResult<TRes> Apply(TArg context = default(TArg));

        public abstract Task<IApplyResult<TRes>> ApplyAsync(TArg context = default(TArg));

        public void Configure(XmlNode node, IConfiguration<TArg, TRes> context)
        {
            Name = node.Attributes["name"]?.Value;

            _log = context.Log;
            _log.Log(LogLevel.Debug, string.Format("Configuring rule '{0}' of type '{1}'", Name, GetType().FullName));

            _keys.Clear();
            var keySection = node.SelectSingleNode("keys");
            foreach (XmlNode nKey in keySection)
            {
                var key = nKey.Value;
                _keys.Add(key);
            }
        }
    }
}
