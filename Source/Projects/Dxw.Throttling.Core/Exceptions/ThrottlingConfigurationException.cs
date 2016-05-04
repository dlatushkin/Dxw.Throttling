namespace Dxw.Throttling.Core.Exceptions
{
    public class ThrottlingConfigurationException: ThrottlingException
    {
        public ThrottlingConfigurationException(string msg): base(msg) {}
    }
}
