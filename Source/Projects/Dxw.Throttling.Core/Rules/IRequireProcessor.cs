namespace Dxw.Throttling.Core.Rules
{
    using Dxw.Throttling.Core.Processor;

    public interface IRequireProcessor
    {
        IProcessor Processor { set; }
    }
}
