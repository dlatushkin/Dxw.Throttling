namespace Dxw.Throttling.Core.Configuration
{
    using System;
    using System.Configuration;
    using System.Xml;

    public class ThrottleConfiguration : ConfigurationSection//, IConfigurationSectionHandler
    {
        public ThrottleConfiguration()
        {

        }

        //[ConfigurationProperty("nodes", IsDefaultCollection=false, IsRequired=false)]
        //public ThrottleConfigurationNodeCollection Nodes
        //{
        //    get
        //    {
        //        var nodes = (ThrottleConfigurationNodeCollection)base["nodes"];
        //        return nodes;
        //    }
        //}
        //public object Create(object parent, object configContext, XmlNode section)
        //{
        //    //throw new NotImplementedException();
        //}
    }
}
