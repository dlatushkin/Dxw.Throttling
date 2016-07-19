namespace Dxw.Throttling.Core.Configuration
{
    using System.Collections.Generic;
    using Rules;
    using Storages;

    public interface IConfiguration
    {
        IEnumerable<IStorage> Storages { get; }
    }

    public interface IConfiguration<out T>: IConfiguration
    {
        IEnumerable<IRule<T>> Rules { get; }
        IRule<T> Rule { get; }
    }
}
