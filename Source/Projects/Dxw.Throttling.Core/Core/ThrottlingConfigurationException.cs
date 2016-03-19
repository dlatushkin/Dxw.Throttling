namespace Dxw.Throttling.Core
{
    public class ThrottlingConfigurationException: ThrottlingException
    {
        public ThrottlingConfigurationException(string msg): base(msg) {}
    }
}
