namespace Dxw.Throttling.Core.Processors
{
    using Rules;

    public interface IRuledProcessor<out T>: IProcessor<T>
    {
        /// <summary>
        /// Result that contains reference to its rule
        /// </summary>
        IRule<T> Rule { get; }
    }
}
