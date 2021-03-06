﻿namespace Dxw.Throttling.UnitTests.Rules
{
    using System;
    using System.Threading.Tasks;
    using Dxw.Throttling.Core.Rules;

    public class TimeOfDayDegreeRule : IRule<DateTime, byte>
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

        public Task<IApplyResult<byte>> ApplyAsync(DateTime context = default(DateTime))
        {
            var result = Apply(context);
            return Task.FromResult(result);
        }
    }
}
