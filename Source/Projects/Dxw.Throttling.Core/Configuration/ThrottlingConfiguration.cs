﻿namespace Dxw.Throttling.Core.Configuration
{
    using System.Linq;
    using System.Collections.Generic;

    using Rules;
    using Storages;
    using Logging;

    public class ThrottlingConfiguration<TRes, TArg> : IConfiguration<TRes, TArg>
    {
        private IEnumerable<ILog> _logs;
        public IEnumerable<ILog> Logs
        {
            get { return _logs ?? Enumerable.Empty<ILog>(); }
            set { _logs = value; }
        }

        public ILog Log { get { return Logs.FirstOrDefault(); } }

        private IEnumerable<IStorage> _storages;
        public IEnumerable<IStorage> Storages
        {
            get { return _storages ?? Enumerable.Empty<IStorage>(); }
            set { _storages = value; }
        }

        private IRule<TRes, TArg> _rule;
        public IRule<TRes, TArg> Rule
        {
            get { return _rule ?? Rules.FirstOrDefault(); }
            set { _rule = value; }
        }

        private IEnumerable<IRule<TRes, TArg>> _rules;
        public IEnumerable<IRule<TRes, TArg>> Rules
        {
            get { return _rules ?? Enumerable.Empty<IRule<TRes, TArg>>(); }
            set { _rules = value; }
        }

        public IRule<TRes, TArg> GetRuleByName(string name = null)
        {
            var rule = string.IsNullOrWhiteSpace(name) ? Rule : _rules.FirstOrDefault(r => r.Name == name);
            return rule;
        }
    }
}
