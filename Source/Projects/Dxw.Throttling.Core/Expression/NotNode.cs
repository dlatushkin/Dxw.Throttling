namespace Dxw.Throttling.Core.Expression
{
    using System;
    using System.Net.Http;

    public class NotNode : INode
    {
        private readonly INode _childNode;

        public string Hit(HttpRequestMessage msg, DateTime utcNow)
        {
            throw new NotImplementedException();
        }
    }
}
