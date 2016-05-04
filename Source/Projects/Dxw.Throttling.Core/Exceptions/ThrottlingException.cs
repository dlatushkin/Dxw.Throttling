namespace Dxw.Throttling.Core.Exceptions
{
    using System;

    public class ThrottlingException : Exception
    {
        public ThrottlingException(string msg): base(msg) {}
    }
}
