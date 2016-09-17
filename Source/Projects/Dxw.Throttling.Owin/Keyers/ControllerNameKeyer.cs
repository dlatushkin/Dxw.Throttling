namespace Dxw.Throttling.Owin.Keyers
{
    using System.Linq;
    using Microsoft.Owin;
    using Core.Keyers;
    using Core;
    using Core.Logging;
    using System.Xml;
    using Core.Configuration;

    public class ControllerNameKeyer : IKeyer<IOwinArgs>, IXmlConfigurable
    {
        private ILog _log;

        public object GetKey(IOwinArgs owinArgs)
        {
            var controller = owinArgs.OwinContext.Request.Path.Value.Split('/').Last();
            return controller;
        }

        public void Configure(XmlNode node, IConfiguration context)
        {
            _log = context.Log;
            _log.Log(LogLevel.Debug, string.Format("Configuring keyer of type '{0}'", GetType().FullName));
        }
    }
}
