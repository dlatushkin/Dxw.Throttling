using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dxw.Throttling.Core.Keyer;
using Dxw.Throttling.Core.Storage;

namespace Dxw.Throttling.Core.Rules
{
    public class RuleNode : IRule
    {


        public IApplyResult Apply(IRequestContext context = null, IStorage storage = null)
        {
            throw new NotImplementedException();
        }
    }
}
