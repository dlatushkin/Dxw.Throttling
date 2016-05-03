namespace Dxw.Throttling.Core.Configuration
{
    using System.Collections.Generic;
    using Rules;
    using Storage;

    public interface IConfiguratedRules
    {
        IEnumerable<IStorage> Storages { get; }
        IEnumerable<IRule> Rules { get; }
        IRule Rule { get; }
    }
}
