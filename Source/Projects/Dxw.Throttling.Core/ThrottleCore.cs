using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Throttling
{
    public class ThrottlingQuota
    {
        public readonly int RangeSeconds;
        public readonly int MaxHits;

        public ThrottlingQuota(int rangeSeconds, int maxHits)
        {
            RangeSeconds = rangeSeconds;
            MaxHits = maxHits;
        }

        public override bool Equals(object obj)
        {
            var other = obj as ThrottlingQuota;
            if (other == null) return false;
            return RangeSeconds == other.RangeSeconds && MaxHits == other.MaxHits;
        }

        public override int GetHashCode()
        {
            return RangeSeconds ^ MaxHits;
        }

        public override string ToString()
        {
            return $"{MaxHits} / {RangeSeconds} sec.";
        }
    }

    public class ThrottlingRule
    {
        public readonly Func<HttpRequestMessage, object> GetRequestKey;
        public readonly ThrottlingQuota Quota;

        public ThrottlingRule(Func<HttpRequestMessage, object> getRequestKey, ThrottlingQuota quota)
        {
            GetRequestKey = getRequestKey;
            Quota = quota;
        }
    }

    public enum ThrottlingSlotState { LimitOk, LimitExceeded }

    public class ThrottlingSlot
    {
        private readonly ThrottlingQuota _quota;
        private DateTime _expiresAt;
        private int _hits;
        
        public ThrottlingSlotState State;

        public static ThrottlingSlot Hit(ThrottlingQuota quota, DateTime utcNow)
        {
            return new ThrottlingSlot(quota, utcNow).Hit(utcNow);
        }

        private ThrottlingSlot(ThrottlingQuota quota, DateTime utcNow)
        {
            _quota = quota;
            _expiresAt = utcNow.AddSeconds(quota.RangeSeconds);
        }

        public ThrottlingSlot Hit(DateTime utcNow)
        {
            if (IsExpired(utcNow))
            {
                _expiresAt = utcNow.AddSeconds(_quota.RangeSeconds);
                _hits = 0;
            }

            State = (++_hits <= _quota.MaxHits) ? ThrottlingSlotState.LimitOk : ThrottlingSlotState.LimitExceeded;

            return this;
        }

        public bool IsExpired(DateTime utcNow)
        {
            return _expiresAt <= utcNow;
        }

        public override string ToString()
        {
            return $"{State}: {_hits} hits expire at '{_expiresAt}' ({_quota})";
        }
    }

    public class ThrottlingSlotKey
    {
        public readonly ThrottlingQuota Quota;
        public readonly object RequestKey;

        public ThrottlingSlotKey(ThrottlingQuota quota, object requestKey)
        {
            Quota = quota;
            RequestKey = requestKey;
        }

        public override bool Equals(object obj)
        {
            var other = obj as ThrottlingSlotKey;
            if (other == null) return false;
            return Quota == other.Quota && Equals(RequestKey, other.RequestKey);
        }

        public override int GetHashCode()
        {
            return RequestKey.GetHashCode() ^ Quota.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Quota} of {RequestKey}";
        }
    }

    public class ThrottleCore: IDisposable
    {
        private IEnumerable<ThrottlingRule> _rules;
        private IThrottlerStorage _storage;

        public ThrottleCore(IEnumerable<ThrottlingRule> rules, IThrottlerStorage storage = null)
        {
            _rules = rules ?? Enumerable.Empty<ThrottlingRule>();
            _storage = storage ?? new ThrottlingStorageConcurrent();
        }

        public string CheckRequest(HttpRequestMessage request)
        {
            var utcNow = DateTime.Now;

            foreach (var rule in _rules)
            {
                var requestKey = rule.GetRequestKey(request);
                if (requestKey == null) continue;

                var slotKey = new ThrottlingSlotKey(rule.Quota, requestKey);
                var state = _storage.Hit(slotKey, utcNow);
                if (state == ThrottlingSlotState.LimitExceeded)
                    return $"Hit quota execeeded. Allowed {rule.Quota.MaxHits} per {rule.Quota.RangeSeconds} seconds.";
            }
            return null;
        }

        public void Dispose()
        {
            _rules = null;

            _storage.Dispose();
            _storage = null;
        }
    }
}
