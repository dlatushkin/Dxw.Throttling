namespace Dxw.Throttling.Core.Rules
{
    using Dxw.Throttling.Core.Processors;

    public interface IRequireProcessor
    {
        IProcessor Processor { set; }
    }
}
