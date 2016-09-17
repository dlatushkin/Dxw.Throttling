namespace Dxw.Throttling.Asp.Keyers
{
    using System.Net.Http;

    using Core.Keyers;
    using Core;
    using Core.Logging;
    using Core.Configuration;
    using System.Xml;

    public class URIMethodKeyer : IKeyer<IAspArgs>, IXmlConfigurable
    {
        private ILog _log;

        public object GetKey(IAspArgs args)
        {
            var request = args.Request;
            return new { request.RequestUri, request.Method };
        }

        public void Configure(XmlNode node, IConfiguration context)
        {
            _log = context.Log;
            _log.Log(LogLevel.Debug, string.Format("Configuring keyer of type '{0}'", GetType().FullName));
        }
    }
}
