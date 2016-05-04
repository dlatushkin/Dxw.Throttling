namespace Dxw.Throttling.Core.Exceptions
{
    public class ThrottlingRuleException : ThrottlingException
    {
        public ThrottlingRuleException(string msg): base(msg) {}
    }
}
