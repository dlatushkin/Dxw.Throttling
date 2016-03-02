namespace Dxw.Throttling.Core
{
    using Keyer;

    public class ThrottlingRule
    {
        //public readonly Func<HttpRequestMessage, object> GetRequestKey;
        public readonly IKeyer Keyer;
        public readonly ThrottlingQuota Quota;

        //public ThrottlingRule(Func<HttpRequestMessage, object> getRequestKey, ThrottlingQuota quota)
        public ThrottlingRule(IKeyer keyer, ThrottlingQuota quota)
        {
            //GetRequestKey = getRequestKey;
            Keyer = keyer;
            Quota = quota;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Quota.ToString(), Keyer.GetType().Name);
        }
    }
}
