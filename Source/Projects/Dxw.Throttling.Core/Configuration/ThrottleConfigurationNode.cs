//namespace Dxw.Throttling.Core.Configuration
//{
//    using System.Configuration;

//    public enum ThrottleConfigurationNodeKind { Func, Or, And, Not };

//    public class ThrottleConfigurationNode : ConfigurationElement
//    {
//        public ThrottleConfigurationNode()
//        {

//        }

//        [ConfigurationProperty("kind", IsRequired = true)]
//        public ThrottleConfigurationNodeKind Kind
//        {
//            get { return (ThrottleConfigurationNodeKind)this["kind"]; }
//            set { this["kind"] = value; }
//        }

//        [ConfigurationProperty("name", IsRequired = true)]
//        public string Name
//        {
//            get { return (string)this["name"]; }
//            set { this["name"] = value; }
//        }
//    }
//}
