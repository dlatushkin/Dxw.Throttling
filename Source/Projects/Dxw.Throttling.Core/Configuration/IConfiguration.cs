namespace Dxw.Throttling.Core.Configuration
{
    using System.Collections.Generic;
    using Rules;
    using Storages;

    public interface IConfiguration
    {
        IEnumerable<IStorage> Storages { get; }
    }

    public interface IConfiguration<out T, in TArg>: IConfiguration
    {
        IEnumerable<IRule<T, TArg>> Rules { get; }
        IRule<T, TArg> Rule { get; }
    }
}
