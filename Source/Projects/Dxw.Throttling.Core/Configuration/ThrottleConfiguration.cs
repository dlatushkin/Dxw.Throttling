namespace Dxw.Throttling.Core.Configuration
{
    using Keyer;
    using Expression;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Xml;

    public class ThrottleConfiguration : IConfigurationSectionHandler
    {
        private const string QuotaNodeName = "quota";

        private const string KeyerTypeName = "type";
        private const string MaxHitsAttrName = "maxHits";
        private const string RangeSecondsAttrName = "perSeconds";

        public object Create(object parent, object configContext, XmlNode section)
        {
            var childrenCount = section.ChildNodes.Count;

            if (childrenCount == 0) return new ConstantKeyer();

            if (childrenCount == 1) return Build(section.FirstChild);

            return CreateAndNode(section);
        }

        private INode Build(XmlNode xmlNode)
        {
            INode resultNode = null;

            if (string.Compare(AndNode.NodeName, xmlNode.Name, true) == 0)
            {
                resultNode = CreateAndNode(xmlNode);
            } 
            else if (string.Compare(QuotaNodeName, xmlNode.Name, true) == 0)
            {
                resultNode = CreateQuotaNode(xmlNode);
            }

            return resultNode;
        }

        private AndNode CreateAndNode(XmlNode andXmlNode)
        {
            var childNodeList = new List<INode>();
            foreach (XmlNode xmlChild in andXmlNode.ChildNodes)
            {
                var childNode = Build(xmlChild);
                childNodeList.Add(childNode);
            }

            return new AndNode(childNodeList);
        }

        private QuotaNode CreateQuotaNode(XmlNode quotaXmlNode)
        {
            var maxHits = Convert.ToInt32(quotaXmlNode.Attributes[MaxHitsAttrName].Value);
            var rangeSeconds = Convert.ToInt32(quotaXmlNode.Attributes[RangeSecondsAttrName].Value);

            var keyerTypeName = quotaXmlNode.Attributes[KeyerTypeName].Value;

            var keyerType = Type.GetType(keyerTypeName, false);
            if (keyerType == null)
                throw new ThrottlingConfigurationException(string.Format("Couldn't create instance of type '{0}'", keyerTypeName));

            if (!typeof(IKeyer).IsAssignableFrom(keyerType))
                throw new ThrottlingConfigurationException(string.Format("Type isn't '{0}' descendant", typeof(IKeyer)));

            var keyer = Activator.CreateInstance(keyerType) as IKeyer;
            var quota = new ThrottlingQuota(rangeSeconds, maxHits);
            var rule = new ThrottlingRule(keyer, quota);

            return new QuotaNode(rule);
        }
    }
}
