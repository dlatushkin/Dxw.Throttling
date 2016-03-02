namespace Dxw.Throttling.Core.Expression
{
    using System;
    using System.Net.Http;

    public interface INode
    {
        string Hit(HttpRequestMessage msg, DateTime utcNow);
    }
}
