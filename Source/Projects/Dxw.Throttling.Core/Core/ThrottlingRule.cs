namespace Dxw.Throttling.Core
{
    using Keyer;

    public class ThrottlingRule
    {
        public readonly IKeyer Keyer;
        public readonly ThrottlingQuota Quota;

        public ThrottlingRule(IKeyer keyer, ThrottlingQuota quota)
        {
            Keyer = keyer;
            Quota = quota;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Quota.ToString(), Keyer.GetType().Name);
        }
    }
}
