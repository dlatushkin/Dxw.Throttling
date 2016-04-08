namespace Dxw.Throttling.Core.Rules
{
    using EventProcessor;

    public interface IRequireProcessor
    {
        IEventProcessor Processor { set; }
    }
}
