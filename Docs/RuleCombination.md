# Rules combination

The most natural way of constructing complex rule is combination of primitive rules.
Dxw.Throttling contains [RuleAndNode<TArg>](../Source/Projects/Dxw.Throttling.Core/Rules/RuleAndNode.cs) 
and [RuleOrNode<TArg>](../Source/Projects/Dxw.Throttling.Core/Rules/RuleOrNode.cs) classes.
They allow to aggregate multiple primitive or complex rules.
Result is logical "and" / "or" combination on child rules.
Also Asp and Owin versions of the rules are located in corresponding projects.


