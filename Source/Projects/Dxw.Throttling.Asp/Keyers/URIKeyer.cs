namespace Dxw.Throttling.Asp.Keyers
{
    using Core.Keyers;

    public class URIKeyer : IKeyer<IAspArgs>
    {
        public object GetKey(IAspArgs args)
        {
            return args.Request.RequestUri;
        }
    }
}
