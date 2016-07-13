﻿namespace Dxw.Throttling.Core.Rules
{
    using Dxw.Throttling.Core.Processors;

    public interface IRequireProcessor<in T>
    {
        IProcessor<T> Processor { set; }
    }
}
