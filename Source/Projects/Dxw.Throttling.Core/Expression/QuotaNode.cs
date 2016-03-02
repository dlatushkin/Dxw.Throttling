namespace Dxw.Throttling.Core.Expression
{
    using System;
    using System.Net.Http;

    public class QuotaNode : INode
    {
        private readonly ThrottlingRule _rule;

        public QuotaNode(ThrottlingRule rule)
        {
            _rule = rule;
        }

        public string Hit(HttpRequestMessage msg, DateTime utcNow)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", typeof(QuotaNode).Name, _rule);
        }
    }
}
