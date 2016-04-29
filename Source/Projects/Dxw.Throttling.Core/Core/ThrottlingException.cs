namespace Dxw.Throttling.Core
{
    using System;

    public class ThrottlingException : Exception
    {
        public ThrottlingException(string msg): base(msg) {}
    }
}
