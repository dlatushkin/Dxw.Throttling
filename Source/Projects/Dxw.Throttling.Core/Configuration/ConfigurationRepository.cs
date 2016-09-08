namespace Dxw.Throttling.Core.Configuration
{
    using System.Collections.Concurrent;
    using System.Configuration;

    public static class ConfigurationRepository<TRes, TArg>
    {
        private static ConcurrentDictionary<string, IConfiguration<TRes, TArg>> _configurations = 
            new ConcurrentDictionary<string, IConfiguration<TRes, TArg>>();

        public static IConfiguration<TRes, TArg> GetSection(string sectionName)
        {
            var configuration = _configurations.GetOrAdd(sectionName, sn =>
            {
                var section = ConfigurationManager.GetSection(sn) as IConfiguration<TRes, TArg>;
                return section;
            });

            return configuration;
        }
    }
}
