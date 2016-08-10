# Dxw.Throttling

## Dxw.Throttling library is in progress

Dxw.Throttling is a throttling library/framework that can be used in WebAPI and Owin self-hosted applications.
It's designed to achieve maximal flexibility and extensibility.
Dxw.Throttling can be easilty customized for the most complex and sophisticated tasks.
From one hand it can be overriden on any level.
From other hand current codebase allows to implement only neсessary logic w/o writing routine code.

### Standard features
- Asp.Net handler and filter as well as Owin middleware.
- Throttle by hits per period.
- Throttling can be performed based on client IP or server URL criteria.
- Configuration can be done using special section of application config file or via run time code
elements.
- Local in-memory storage.

### Unique features
- Multiple rules can be applied in combination using built-in logical and/or operators.
- Arbitary result types are allowed.
- Pre- / Post- rule process phase are supported.
- Redis/Lua storage is implemented.
- [Open rule model.](OpenRuleModel.md)

### Dxw.Throttling as a ready-to-use library

The most common use cases are already implemented and can be used "out-of-the-box".
Let's review trivial IP throttling implementation:

##### Asp.Net Web Api usage
``` cs
public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        var storage = new LocalMemoryStorage();
        var keyer = new ControllerNameKeyer();
        var processor = new RequestCountPerPeriodProcessorBlockPass { Count = 1, Period = TimeSpan.FromSeconds(10) };
        var ruleBlock = new StorageKeyerProcessorRule<PassBlockVerdict, HttpRequestMessage> { Storage = storage, Keyer = keyer, Processor = processor } as IRule;
        var throttlingHandler = new ThrottlingHandler(ruleBlock);
        config.MessageHandlers.Add(throttlingHandler);

        config.MapHttpAttributeRoutes();

        config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
        );
    }
}
```
##### Owin self-hosted app usage
``` cs
public class Startup
{
    public void Configuration(IAppBuilder appBuilder)
    {
        var config = new HttpConfiguration();

        config.Routes.MapHttpRoute("Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });

        var storage = new LocalMemoryStorage();

        var keyer = new ControllerNameKeyer();
        var processor = new RequestCountPerPeriodProcessorBlockPass { Count = 1, Period = TimeSpan.FromSeconds(15) };
        var ruleBlock = new StorageKeyerProcessorRule<PassBlockVerdict, IOwinRequest> { Storage = storage, Keyer = keyer, Processor = processor };

        appBuilder.Use(typeof(ThrottlingPassBlockMiddleware), ruleBlock);

        appBuilder.UseWebApi(config);
    }
}
```

### Dxw.Throttling 

### Dxw.Throttling as a throttling framework
Dxw.Throttling is extremelly extensible throttling framework.

On the other hand Dxw.Throttling is ready-to-use throttling library build for WebAPI and Owin self-hosted applications.
