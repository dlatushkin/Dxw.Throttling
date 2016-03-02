namespace Dxw.Throttling.Core
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
}
