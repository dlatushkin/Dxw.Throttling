namespace Dxw.Throttling.Core.Rules
{
    using System;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Collections.Generic;

    using Configuration;
    using Keyers;
    using Logging;

    public class KeyListPassBlockRule<TArg> : IRule<TArg, PassBlockVerdict>
    {
        public IKeyer<TArg> Keyer { get; set; }

        private ILog _log;

        public string Name { get; private set; }

        public IApplyResult<PassBlockVerdict> Apply(TArg context = default(TArg))
        {
            var key = Keyer.GetKey(context);

            throw new NotImplementedException();
            //return Has
        }

        public Task<IApplyResult<PassBlockVerdict>> ApplyAsync(TArg context = default(TArg))
        {
            throw new NotImplementedException();
        }

        //public override IApplyResult<PassBlockVerdict> Apply(TArg context = default(TArg));
        //{
        //    var key = Keyer.GetKey(context);

        //    if (_blackList.Contains(key))
        //    {

        //    }
        //}

        //public abstract Task<IApplyResult<PassBlockVerdict>> ApplyAsync(TArg context = default(TArg));
    }
}

