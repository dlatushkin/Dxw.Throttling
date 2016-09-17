namespace Dxw.Throttling.Owin.Keyers
{
    using Core;
    using Core.Configuration;
    using Core.Keyers;
    using Core.Logging;
    using System.Xml;

    public class URIMethodKeyer : IKeyer<IOwinArgs>, IXmlConfigurable
    {
        private ILog _log;

        public object GetKey(IOwinArgs owinArgs)
        {
            var owinRequest = owinArgs.OwinContext.Request;

            return new { owinRequest.Uri, owinRequest.Method };
        }

        public void Configure(XmlNode node, IConfiguration context)
        {
            _log = context.Log;
            _log.Log(LogLevel.Debug, string.Format("Configuring keyer of type '{0}'", GetType().FullName));
        }
    }
}
