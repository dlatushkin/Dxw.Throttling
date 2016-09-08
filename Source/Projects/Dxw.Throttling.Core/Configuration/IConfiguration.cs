namespace Dxw.Throttling.Core.Configuration
{
    using System.Collections.Generic;
    using Rules;
    using Storages;
    using Logging;

    public interface IConfiguration
    {
        ILog Log { get; }

        IEnumerable<IStorage> Storages { get; }
    }

    public interface IConfiguration<in TArg, T> : IConfiguration
    {
        IEnumerable<IRule<TArg, T>> Rules { get; }

        IRule<TArg, T> Rule { get; }

        IRule<TArg, T> GetRuleByName(string ruleName = null);
    }
}
