namespace Dxw.Throttling.Core
{
    using System;

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
}
