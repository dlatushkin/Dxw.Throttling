# Arbitary result types

What if "pass" or "block" verdicts only are not enough to describe throttling result?
For example degree of blocking (saying 50%) should be described.
Basically PassBlockVerdict enum is used to describe throttling decision.
But base framework classes are designed as generics.
In particular it allows to operate with arbitary result types.
May be this case is a little bit far-fetched but "never say never".

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