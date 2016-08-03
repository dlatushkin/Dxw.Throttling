namespace Dxw.Throttling.Owin.Keyers
{
    using Microsoft.Owin;

    using Core.Keyers;
    using Core.Exceptions;

    public class URIMethodKeyer : IKeyer<IOwinContext>
    {
        public object GetKey(IOwinContext owinContext)
        {
            if (owinContext == null)
                throw new ThrottlingRuleException("Context should be an instance of IOwinContext.");

            var owinRequest = owinContext.Request;
            if (owinRequest == null)
                throw new ThrottlingRuleException("Context should contain an instance of IOwinRequest.");

            return new { owinRequest.Uri, owinRequest.Method };
        }
    }
}
