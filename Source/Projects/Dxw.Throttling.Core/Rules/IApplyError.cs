namespace Dxw.Throttling.Core.Rules
{
    using System;

    public interface IApplyError
    {
        string Message { get; }
        Exception Error { get; }
    }

    public class ApplyError : IApplyError
    {
        public string Message { get; set; }
        public Exception Error { get; set; }
    }
}
