namespace Dxw.Throttling.Owin.Keyers
{
    using Core.Keyers;

    public class URIKeyer : IKeyer<IOwinArgs>
    {
        public object GetKey(IOwinArgs owinArgs)
        {
            return owinArgs.OwinContext.Request.Uri;
        }
    }
}
