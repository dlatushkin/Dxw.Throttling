namespace Dxw.Throttling.Core.Configuration
{
    using System.Linq;
    using System.Collections.Generic;

    using Rules;
    using Storages;

    public class ThrottlingConfiguration<T, TArg> : IConfiguration<T, TArg>
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

        private IRule<T, TArg> _rule;
        public IRule<T, TArg> Rule
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

        private IEnumerable<IRule<T, TArg>> _rules;
        public IEnumerable<IRule<T, TArg>> Rules
        {
            get
            {
                return _rules ?? Enumerable.Empty<IRule<T, TArg>>();
            }
            set
            {
                _rules = value;
            }
        }
    }
}
