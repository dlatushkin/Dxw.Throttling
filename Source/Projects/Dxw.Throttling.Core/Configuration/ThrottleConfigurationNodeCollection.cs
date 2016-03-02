using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dxw.Throttling.Core.Configuration
{
    public class ThrottleConfigurationNodeCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ThrottleConfigurationNode();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ThrottleConfigurationNode)element).Name;
        }
    }
}
