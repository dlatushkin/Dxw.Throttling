namespace Dxw.Throttling.Core.Configuration
{
    using System.Linq;
    using System.Collections.Generic;

    using Rules;
    using Storage;

    public class ThrottlingConfiguration : IConfiguration
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

        private IRule _rule;
        public IRule Rule
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

        private IEnumerable<IRule> _rules;
        public IEnumerable<IRule> Rules
        {
            get
            {
                return _rules ?? Enumerable.Empty<IRule>();
            }
            set
            {
                _rules = value;
            }
        }
    }
}
