namespace Dxw.Throttling.Core.Rules
{
    using System;
    using Dxw.Throttling.Core.Keyer;
    using Dxw.Throttling.Core.Storage;

    public class RuleBase : IRule
    {
        private readonly IKeyer _keyer;
        //private readonly ISlotCalculator _slotCalculator;

        public RuleBase(IKeyer keyer, ISlotCalculator slotCalculator)
        {
            _keyer = keyer;
            //_slotCalculator = slotCalculator;
        }

        public IApplyResult Apply(IRequestContext context, IStorage storage)
        {
            var key = _keyer.GetKey(context);

            //var slotObj = storage.Upsert(key, _slotCalculator);

            //var result = _slotCalculator.CalculateResult(slotObj);

            //return result;

            throw new NotImplementedException();
        }
    }
}
