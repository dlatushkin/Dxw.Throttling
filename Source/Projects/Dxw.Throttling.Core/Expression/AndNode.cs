namespace Dxw.Throttling.Core.Expression
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;

    public class AndNode : INode
    {
        public const string NodeName = "and";

        private readonly IEnumerable<INode> _children;

        public AndNode(IEnumerable<INode> children)
        {
            _children = children;
        }

        public string Hit(HttpRequestMessage request, DateTime utcNow)
        {
            var results = _children.Select(ch => ch.Hit(request, utcNow));
            return results.FirstOrDefault(r => r != null);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(NodeName);

            foreach (var child in _children)
                sb.AppendLine("  " + child);

            return sb.ToString();
        }
    }
}
