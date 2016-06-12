namespace Dxw.Throttling.Core.Processors
{
    using Rules;
        
    public interface IProcessor
    {
        /// <summary>
        /// Processes an event
        /// </summary>
        /// <param name="key">Key of the operation.</param>
        /// <param name="context">Event context (e.g. HttpRequestMessage).</param>
        /// <param name="storage">Storage instance.</param>
        /// <param name="rule">Caller</param>
        /// <returns></returns>
        IProcessEventResult Process(object key = null, object context = null, object storeEndpoint = null, IRule rule = null);
    }
}
