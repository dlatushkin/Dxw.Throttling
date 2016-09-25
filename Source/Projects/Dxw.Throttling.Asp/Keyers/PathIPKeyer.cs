namespace Dxw.Throttling.Asp.Keyers
{
    using System;

    public class PathIPKeyer : IPKeyer
    {
        public class PathIPKey
        {
            private readonly string _ip;
            private readonly string _path;

            public PathIPKey(string ip, IAspArgs context)
            {
                _ip = ip;
                _path = context.Request.RequestUri.AbsolutePath;
            }

            public override bool Equals(object obj)
            {
                var other = obj as PathIPKey;
                if (other == null) return false;
                return _ip == other._ip && string.Equals(_path, other._path, StringComparison.CurrentCultureIgnoreCase);
            }

            public override int GetHashCode()
            {
                return _ip.GetHashCode() ^ _path.GetHashCode();
            }

            public override string ToString()
            {
                return _ip + ":" + _path;
            }
        }

        public override object GetKey(IAspArgs context)
        {
            var ip = GetIP(context);
            return new PathIPKey(ip, context);
        }
    }
}
