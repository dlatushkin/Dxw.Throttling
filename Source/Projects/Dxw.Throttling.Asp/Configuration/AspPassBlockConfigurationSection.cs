namespace Dxw.Throttling.Asp.Configuration
{
    using Core.Rules;
    using Dxw.Throttling.Core.Configuration;

    public class AspPassBlockConfigurationSection: ThrottlingConfiguration<IAspArgs, PassBlockVerdict> {}
}
