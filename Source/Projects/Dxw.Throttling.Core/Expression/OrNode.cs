namespace Dxw.Throttling.Core.Expression
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    public class OrNode : INode
    {
        private readonly IEnumerable<INode> _children;

        public OrNode(IEnumerable<INode> children)
        {
            _children = children;
        }

        public string Hit(HttpRequestMessage request, DateTime utcNow)
        {
            var results = _children.Select(ch => ch.Hit(request, utcNow));
            return results.FirstOrDefault(r => r == null);
        }
    }
}
