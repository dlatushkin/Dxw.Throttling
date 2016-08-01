namespace Dxw.Throttling.Core.Configuration
{
    using System.Linq;
    using System.Collections.Generic;

    using Rules;
    using Storages;

    public class ThrottlingConfiguration<TRes, TArg> : IConfiguration<TRes, TArg>
    {
        private IEnumerable<IStorage> _storages;
        public IEnumerable<IStorage> Storages
        {
            get
            {
                return _storages ?? Enumerable.Empty<IStorage>();
            }
            set
            {
                _storages = value;
            }
        }

        private IRule<TRes, TArg> _rule;
        public IRule<TRes, TArg> Rule
        {
            get
            {
                return _rule ?? Rules.FirstOrDefault();
            }
            set
            {
                _rule = value;
            }
        }

        private IEnumerable<IRule<TRes, TArg>> _rules;
        public IEnumerable<IRule<TRes, TArg>> Rules
        {
            get
            {
                return _rules ?? Enumerable.Empty<IRule<TRes, TArg>>();
            }
            set
            {
                _rules = value;
            }
        }
    }
}
