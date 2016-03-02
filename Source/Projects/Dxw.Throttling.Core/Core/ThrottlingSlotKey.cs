namespace Dxw.Throttling.Core
{
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
}
