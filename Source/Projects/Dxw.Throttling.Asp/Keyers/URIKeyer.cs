namespace Dxw.Throttling.Asp.Keyers
{
    using System.Net.Http;

    using Microsoft.Owin;
    
    using Core.Keyers;
    using Core.Exceptions;

    public class URIKeyer : IKeyer
    {
        public object GetKey(object context)
        {
            var owinContext = context as OwinRequest;
            if (owinContext != null)
                return owinContext.Uri;

            var request = context as HttpRequestMessage;
            if (request != null)
                return request.RequestUri;

            throw new ThrottlingRuleException("Nor OwinRequest neither HttpRequestMessage are set in context argument");
        }
    }
}
