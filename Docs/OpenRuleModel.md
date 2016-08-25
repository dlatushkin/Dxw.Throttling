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
Framework contains several implementations of these interfaces:


