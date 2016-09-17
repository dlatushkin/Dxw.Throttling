namespace Dxw.Throttling.Asp.Keyers
{
    using System;
    using System.Xml;
    using System.Net.Http;

    using Core.Keyers;
    using Core.Logging;
    using Core;
    using Core.Configuration;

    public class ControllerNameKeyer : IKeyer<IAspArgs>, IXmlConfigurable
    {
        private ILog _log;

        public object GetKey(IAspArgs args)
        {
            var request = args.Request;

            var routeData = request.GetRouteData();

            var controllerName = (string)routeData.Values["controller"];

            return controllerName;
        }

        public void Configure(XmlNode node, IConfiguration context)
        {
            _log = context.Log;
            _log.Log(LogLevel.Debug, string.Format("Configuring keyer of type '{0}'", GetType().FullName));
        }
    }
}
