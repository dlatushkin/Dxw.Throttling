# Redis / Lua storage

Nowadays multiple nodes can be used to achieve greater performance. 
Hence Dxw.Throttling supports network storage to support such configuration.
Redis Storage implementation is located in Dxw.Throttling.Redis nuget package.
Below sample of using this storage is listed.
Xml configuration code:
``` xml
...
<configSections>
        <section name="throttling" 
                 type="Dxw.Throttling.Core.Configuration.ConfigurationSectionHandler`2[[Dxw.Throttling.Core.Rules.PassBlockVerdict, Dxw.Throttling.Core, Version=1.0.0.8, Culture=neutral, PublicKeyToken=null],[System.Object, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], Dxw.Throttling.Core, Version=1.0.0.8, Culture=neutral, PublicKeyToken=null" />
    </configSections>
    
    <throttling>
        <storages>
            <storage type="Dxw.Throttling.Redis.Storages.RedisStorage, Dxw.Throttling.Redis" name="redis" 
                     connectionString ="localhost" />
        </storages>
        <rules>
            <rule type="Dxw.Throttling.Core.Rules.StorageKeyerProcessorPassBlockRule" name="singleRedis">
                <storage name="redis" />
                <keyer type="Dxw.Throttling.Core.Keyers.ConstantKeyer" />
                <processor type="Dxw.Throttling.Redis.Processors.RequestCountPerPeriodProcessorBlockPass, Dxw.Throttling.Redis"
                           count="3" period="00:00:10" />
            </rule>
        </rules>
    </throttling>
...
```
corresponding c# code:
``` cs
var throttlingConfiguration = ConfigurationManager.GetSection("throttling") as ThrottlingConfiguration<PassBlockVerdict, object>;
var redisRule = throttlingConfiguration.Rule;
```
