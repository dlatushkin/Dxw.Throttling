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

Below is example of such rule:
``` cs
public class TimeOfDayDegreeRule : IRule<byte, DateTime>
{
    public string Name { get { return GetType().Name; } }

    public IApplyResult<byte> Apply(DateTime context = default(DateTime))
    {
        var hour = context.Hour;
        byte verdict;
        if (hour < 6 || hour > 23)
            verdict = 1;
        else if (hour < 8 || hour > 20)
            verdict = 2;
        else
            verdict = 3;

        return new ApplyResult<byte> { Rule = this, Verdict = verdict };
    }
}
```
and unit test fragment utilizing the rule:
``` cs
...
var rule = new TimeOfDayDegreeRule();

IApplyResult<byte> result;

result = rule.Apply(new DateTime(2016, 8, 16, 2, 0, 0));
Assert.AreEqual(1, result.Verdict);

result = rule.Apply(new DateTime(2016, 8, 16, 7, 0, 0));
Assert.AreEqual(2, result.Verdict);

result = rule.Apply(new DateTime(2016, 8, 16, 14, 0, 0));
Assert.AreEqual(3, result.Verdict);
...
```