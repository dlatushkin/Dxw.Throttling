# Open model

The main feature of Dxw.Throttling is open rule model. So any part of throttling process can be easly customized.
Lets briefly review architecture of the framework.

## IRule implementation
The key interface is [IRule<TRes, TArg>](/Source/Projects/Dxw.Throttling.Core/Rules/IRule.cs).
Implementing this interface is probably the most labour-consuming way.
But from other side this is the most powerfull and the most powerfull and flexible way.
Suppose request should be allowed/forbidden depending on the time of the day.
Below is sample implementation of such rule:
``` cs
public class TooLateTooEarlyRule : IRule<DateTime, PassBlockVerdict>
{
    public virtual string Name { get { return this.GetType().Name; } }

    public IApplyResult<PassBlockVerdict> Apply(DateTime now = default(DateTime))
    {
        if (now.TimeOfDay > new TimeSpan(18, 0, 0))
            return ApplyResultPassBlock.Block(msg: "It's too late now");

        if (now.TimeOfDay < new TimeSpan(6, 0, 0))
            return ApplyResultPassBlock.Block(msg: "It's too early now");

        return ApplyResultPassBlock.Pass(this);
    }
}
```
All requests before 6 AM and after 6 PM are blocked.
Source code can be found [here](/Source/Projects/Dxw.Throttling.Core/Rules/TooLateTooEarlyRule.cs).

## StorageKeyerProcessorRule<TArg, TRes> with various IKeyer/IProcessor/IStorage

[StorageKeyerProcessorRule<TArg, TRes>](/Source/Projects/Dxw.Throttling.Core/Rules/StorageKeyerProcessorRule.cs) represents higher level of framework use.
It's driven by assigning particular keyer, processor and storage.
Therefore different keyers, processors or storages once implemented can be re-used 
in different combinations.
Framework contains several implementations of these interfaces.

### Processors
The most typical case in throttling is limiting query count per period of time 
(e.g. chat application allows maximum 2 messages per second per user).
Throttling framework implements two classes named RequestCountPerPeriodProcessor 
in Dxw.Throttling.Core and Dxw.Throttling.Redis projects for this purpose.
First one uses local memory storage, second one uses Redis server.

### Storages
Respectively two storages are supported: 
- LocalMemoryStorage
- Redis storage.
A storage is just a cotainer for storage object. It can be .Net ConcurrentDictionary instance or
Redis connection string.
More detailed implementation is considered irrelevant because technologies of data 
persistence and, most importantly, modification can be very different.
For example, RedisStorage utilizes [Lua](https://www.redisgreen.net/blog/intro-to-lua-for-redis-programmers/) 
script to modify value directly in Redis server without 
"lock on server -> download to client -> modify -> upload to server -> unlock on server"
routine.
Hence processor implementation is more labour-consuming but much more flexible.

### Keyers 

The keyer is a class projecting rule argument 
([IAspArgs](/Source/Projects/Dxw.Throttling.Asp/IAspArgs.cs) and 
[IOwinArgs](/Source/Projects/Dxw.Throttling.Owin/IOwinArgs.cs) respectively) to "key"
object used to calculate statistics.
Dxw.Throttling implements the following rule:

- Controller name keyer ([Asp](/Source/Projects/Dxw.Throttling.Asp/Keyers/ControllerNameKeyer.cs) and [Owin](/Source/Projects/Dxw.Throttling.Owin/Keyers/ControllerNameKeyer.cs) versions)
- Client IP keyer ([Asp](/Source/Projects/Dxw.Throttling.Asp/Keyers/IPKeyer.cs) and [Owin](/Source/Projects/Dxw.Throttling.Owin/Keyers/IPKeyer.cs) versions)
- Query URI keyer ([Asp](/Source/Projects/Dxw.Throttling.Asp/Keyers/URIKeyer.cs) and [Owin](/Source/Projects/Dxw.Throttling.Owin/Keyers/URIKeyer.cs) versions)
- Query URI & HTTP Method keyer ([Asp](/Source/Projects/Dxw.Throttling.Asp/Keyers/URIMethodKeyer.cs) and [Owin](/Source/Projects/Dxw.Throttling.Owin/Keyers/URIMethodKeyer.cs) versions).


