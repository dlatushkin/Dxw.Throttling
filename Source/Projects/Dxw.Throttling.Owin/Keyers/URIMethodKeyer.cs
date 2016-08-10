namespace Dxw.Throttling.Owin.Keyers
{
    using Core.Keyers;

    public class URIMethodKeyer : IKeyer<IOwinArgs>
    {
        public object GetKey(IOwinArgs owinArgs)
        {
            var owinRequest = owinArgs.OwinContext.Request;

            return new { owinRequest.Uri, owinRequest.Method };
        }
    }
}
