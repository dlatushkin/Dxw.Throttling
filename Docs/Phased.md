# Arbitary result types

What if some query metrics can be evaluated only after response is ready?
For example response size or execution time can be calculated at the end of request
processing.
So it's useful to supply two query phases: "before" and "after".
Client can be limited by response size per period of time (i.e. 10Mb per minute).
Block response can be sent in the "after" blocking phase (meaning phase that caused block condition) 
or during next "before" phase because it can be unreasonable to sent block response
instead of ready regulr response. Anyway this decision should be made by a particular developer
in a particular situation.

Does it look like exotic case? May be but "never say never" ).

Below is example of such rule configuration:
``` xml
...
<configSections>
        <section name="throttling" type="Dxw.Throttling.Asp.Configuration.PassBlockConfigurationSectionHandler, Dxw.Throttling.Asp"/>
    </configSections>
    ...
    <throttling>
        <storages>
            <storage type="Dxw.Throttling.Core.Storages.LocalMemoryStorage, Dxw.Throttling.Core" name="local" />
        </storages>
        <rules>
            <rule type="Dxw.Throttling.Asp.Rules.AspStorageKeyerProcessorRule, Dxw.Throttling.Asp" name="size">
                <storage name="local" />
                <keyer type="Dxw.Throttling.Asp.Keyers.ControllerNameKeyer, Dxw.Throttling.Asp" />
                <processor type="Dxw.Throttling.Asp.Processors.ResponseSizeProcessor, Dxw.Throttling.Asp"
                           bytes="20" period="00:00:10" />
            </rule>
        </rules>
    </throttling>
...
```
and corresponding cs code:
``` cs
...
public class FourthController : ApiController
{
    [Throttle(true, "throttling", "size")]
    public string Get()
    {
        return "fourth.get";
    }
}
...
```
The sample can be found in [Dxw.Throttling.WebApiTest](/Source/Projects/Samples/Dxw.Throttling.WebApiTest) project.