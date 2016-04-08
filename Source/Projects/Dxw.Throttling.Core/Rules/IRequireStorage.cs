namespace Dxw.Throttling.Core.Rules
{
    using Storage;

    public interface IRequireStorage
    {
        IStorage Storage { set; }
    }
}
