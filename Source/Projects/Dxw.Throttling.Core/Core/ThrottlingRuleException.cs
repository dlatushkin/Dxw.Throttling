namespace Dxw.Throttling.Core
{
    using System;

    public class ThrottlingRuleException : ThrottlingException
    {
        public ThrottlingRuleException(string msg): base(msg) {}
    }
}
