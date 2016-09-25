namespace Dxw.Throttling.Asp.Keyers
{
    using System.Net.Http;

    public class PathMethodIPKeyer : IPKeyer
    {
        public class PathMethodIPKey: PathIPKeyer.PathIPKey
        {
            private readonly HttpMethod _method;

            public PathMethodIPKey(string ip, IAspArgs context): base(ip, context)
            {
                _method = context.Request.Method;
            }

            public override bool Equals(object obj)
            {
                var other = obj as PathMethodIPKey;
                if (other == null) return false;
                return base.Equals(obj) && _method == other._method;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode() ^ _method.GetHashCode();
            }

            public override string ToString()
            {
                return base.ToString() + "." + _method;
            }
        }

        public override object GetKey(IAspArgs context)
        {
            var ip = GetIP(context);
            return new PathMethodIPKey(ip, context);
        }
    }
}
