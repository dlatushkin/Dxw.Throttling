namespace Dxw.Throttling.Core.Configuration
{
    using System.Linq;
    using System.Collections.Generic;

    using Rules;
    using Storages;

    public class ThrottlingConfiguration<T> : IConfiguration<T>
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

        private IRule<T> _rule;
        public IRule<T> Rule
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

        private IEnumerable<IRule<T>> _rules;
        public IEnumerable<IRule<T>> Rules
        {
            get
            {
                return _rules ?? Enumerable.Empty<IRule<T>>();
            }
            set
            {
                _rules = value;
            }
        }
    }
}
