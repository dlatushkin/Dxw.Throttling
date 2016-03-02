namespace Dxw.Throttling.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using Expression;

    public class ThrottleCore: IDisposable
    {
        //private IEnumerable<ThrottlingRule> _rules;
        private INode _rule;
        private IThrottlerStorage _storage;

        //public ThrottleCore(IEnumerable<ThrottlingRule> rules, IThrottlerStorage storage = null)
        public ThrottleCore(INode rule, IThrottlerStorage storage = null)
        {
            //_rules = rules ?? Enumerable.Empty<ThrottlingRule>();
            _rule = rule;
            _storage = storage ?? new ThrottlingStorageConcurrent();
        }

        public string CheckRequest(HttpRequestMessage request)
        {
            if (_rule == null) return null;

            return _rule.Hit(request, DateTime.Now);

            //foreach (var rule in _rules)
            //{
            //    var requestKey = rule.GetRequestKey(request);
            //    if (requestKey == null) continue;

            //    var slotKey = new ThrottlingSlotKey(rule.Quota, requestKey);
            //    var state = _storage.Hit(slotKey, utcNow);
            //    if (state == ThrottlingSlotState.LimitExceeded)
            //        return $"Hit quota execeeded. Allowed {rule.Quota.MaxHits} per {rule.Quota.RangeSeconds} seconds.";
            //}

            //return null;
        }

        public void Dispose()
        {
            //_rules = null;
            _rule = null;

            _storage.Dispose();
            _storage = null;
        }
    }
}
