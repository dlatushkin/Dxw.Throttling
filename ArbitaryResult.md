# Arbitary result types

What if "pass" or "block" verdicts only are not enough to describe throttling result?
For example degree of blocking (saying 50%) should be described.
Basically PassBlockVerdict enum is used to describe throttling decision.
But base framework classes are designed as generics.
In particular it allows to operate with arbitary result types.
For example Rule of 
