﻿namespace Dxw.Throttling.Core.Processors
{
    using Rules;
    using System.Collections;

    public interface IProcessor<out TRes>
    {
        /// <summary>
        /// Processes an event
        /// </summary>
        /// <param name="key">Key of the operation.</param>
        /// <param name="context">Event context (e.g. HttpRequestMessage).</param>
        /// <param name="storage">Storage instance.</param>
        /// <param name="rule">Caller</param>
        /// <returns></returns>
        IApplyResult<TRes> Process(object key = null, object context = null, object storeEndpoint = null);
    }
}
