# Dxw.Throttling

## Dxw.Throttling library is in progress

Dxw.Throttling is a throttling library/framework that can be used in WebAPI and Owin self-hosted applications.
It's designed to achieve maximal flexibility and extensibility.
Dxw.Throttling can be easilty customized for the most complex and sophisticated tasks.
From one hand it can be overriden on any level.
From other hand current codebase allows to implement only neсessary logic w/o writing routine code.

### Standard features
- Asp.Net handler and filter attribute as well as Owin middleware.
- Throttle by hits per period.
- Throttling can be performed based on client IP or server URL criteria.
- Configuration can be done using special section of application config file or via run time code
elements.
- Local in-memory storage.

### Unique features
- [Multiple rules can be applied in combination using built-in logical and/or operators.](Docs/RuleCombination.md)
- [Arbitary result types are allowed.](Docs/ArbitaryResult.md)
- [Pre- / Post- rule process phase are supported.](Docs/Phased.md)
- [Redis/Lua storage is implemented.](Docs/RedisStorage.md)
- [Open rule model.](Docs/OpenRuleModel.md)

### Dxw.Throttling as a ready-to-use library

The most common use cases are already implemented and can be used "out-of-the-box".
Let's configure trivial IP throttling via code:

Asp.Net Web Api usage as a handler:
``` cs
public static void Register(HttpConfiguration config)
{
    var storage = new LocalMemoryStorage();
    var keyer = new ControllerNameKeyer();
    var processor = new RequestCountPerPeriodProcessorPhased { Count = 1, Period = TimeSpan.FromSeconds(10) };
    var ruleBlock = new StorageKeyerProcessorRule<IAspArgs, PassBlockVerdict> { Storage = storage, Keyer = keyer, Processor = processor } as IRule;
    var throttlingHandler = new ThrottlingHandler(ruleBlock);
    config.MessageHandlers.Add(throttlingHandler);

    config.Routes.MapHttpRoute(
        name: "DefaultApi",
        routeTemplate: "api/{controller}/{id}",
        defaults: new { id = RouteParameter.Optional }
    );
}
```
the same as a attribute:
``` cs
...
public class ThirdController : ApiController
{
    [Throttle]
    public string Get()
    {
        return "third get";
    }

    public string Post()
    {
        return "third post";
    }
}
...
```
Owin self-hosted app usage
``` cs
public void Configuration(IAppBuilder appBuilder)
{
    var config = new HttpConfiguration();

    config.Routes.MapHttpRoute("Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });

    var storage = new LocalMemoryStorage();

    var keyer = new ControllerNameKeyer();
    var processor = new RequestCountPerPeriodProcessorBlockPass { Count = 1, Period = TimeSpan.FromSeconds(15) };
    var ruleBlock = new StorageKeyerProcessorRule<PassBlockVerdict, IOwinArgs> { Storage = storage, Keyer = keyer, Processor = processor };

    appBuilder.Use(typeof(ThrottlingPassBlockMiddleware), ruleBlock);

    appBuilder.UseWebApi(config);
}
```

The same throttling logic but configured via web.config file for Asp.Net:
``` xml
...
<configSections>
        <section name="throttling" type="Dxw.Throttling.Asp.Configuration.PassBlockConfigurationSectionHandler, Dxw.Throttling.Asp"/>
    </configSections>

    <throttling>
        <storages>
            <storage type="Dxw.Throttling.Core.Storages.LocalMemoryStorage, Dxw.Throttling.Core" name="local" />
        </storages>
        <rules>
            <rule type="Dxw.Throttling.Asp.Rules.AspStorageKeyerProcessorRule, Dxw.Throttling.Asp">
                <storage name="local" />
                <keyer type="Dxw.Throttling.Asp.Keyers.ControllerNameKeyer, Dxw.Throttling.Asp" />
                <processor type="Dxw.Throttling.Core.Processors.RequestCountPerPeriodProcessorPhased, Dxw.Throttling.Core" 
                           count="1" period="00:00:10" />
            </rule>
        </rules>
    </throttling>
...
```
and corresponding c# code:
``` cs
public static void Register(HttpConfiguration config)
{
    var throttlingConfig = ConfigurationManager.GetSection("throttling") as Throttling.Core.Configuration.ThrottlingConfiguration<IAspArgs, PassBlockVerdict>;
    var rule = throttlingConfig.Rule;
    var throttlingHandler = new ThrottlingHandler(rule);
    config.MessageHandlers.Add(throttlingHandler);

    config.Routes.MapHttpRoute(
        name: "DefaultApi",
        routeTemplate: "api/{controller}/{id}",
        defaults: new { id = RouteParameter.Optional }
    );
}
```
The same throttling logic but configured via app.config file for Owin:
``` xml
...
<configSections>
        <section name="throttling" type="Dxw.Throttling.Owin.Configuration.PassBlockConfigurationSectionHandler, Dxw.Throttling.Owin"/>
    </configSections>
    <throttling>
        <storages>
            <storage type="Dxw.Throttling.Core.Storages.LocalMemoryStorage, Dxw.Throttling.Core" name="local" />
        </storages>
        <rules>
            <rule type="Dxw.Throttling.Owin.Rules.OwinStorageKeyerProcessorRule, Dxw.Throttling.Owin">
                <storage name="local" />
                <keyer type="Dxw.Throttling.Owin.Keyers.ControllerNameKeyer, Dxw.Throttling.Owin" />
                <processor type="Dxw.Throttling.Core.Processors.RequestCountPerPeriodProcessorPhased, Dxw.Throttling.Core"
                           count="1" period="00:00:10" />
            </rule>
        </rules>
    </throttling>
...
```
and corresponding c# code:
``` cs
public class Startup
{
    public void Configuration(IAppBuilder appBuilder)
    {
        var config = new HttpConfiguration();
        config.Routes.MapHttpRoute("Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });
        var throttlingConfig = ConfigurationManager.GetSection("throttling") as Throttling.Core.Configuration.ThrottlingConfiguration<IOwinArgs, PassBlockVerdict>;
        var rule = throttlingConfig.Rule;

        appBuilder.Use(typeof(ThrottlingPassBlockMiddleware), rule);

        appBuilder.UseWebApi(config);
    }
}
```

### Dxw.Throttling 

### Dxw.Throttling as a throttling framework
Dxw.Throttling is extremelly extensible throttling framework.

On the other hand Dxw.Throttling is ready-to-use throttling library build for WebAPI and Owin self-hosted applications.
