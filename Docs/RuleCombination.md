# Rules combination

The most natural way of constructing complex rule is combination of primitive rules.
Dxw.Throttling contains [RuleAndNode<TArg>](../Source/Projects/Dxw.Throttling.Core/Rules/RuleAndNode.cs) 
and [RuleOrNode<TArg>](../Source/Projects/Dxw.Throttling.Core/Rules/RuleOrNode.cs) classes.
They allow to aggregate multiple primitive or complex rules.
Result is logical "and" / "or" combination on child rules.
Also [Asp](/Source/Projects/Dxw.Throttling.Asp/Rules) and [Owin](/Source/Projects/Dxw.Throttling.Owin/Configuration) versions of the rules are located in corresponding projects.

Below is unit test that demonstrates composite rule nodes using.

Xml configuration:
``` xml
...
<throttling>
        <rules>
            <rule type="Dxw.Throttling.Asp.Rules.AspRuleAndNode, Dxw.Throttling.Asp">
                <rules>
                    <rule type="Dxw.Throttling.Asp.Rules.Constant.AspConstantRule, Dxw.Throttling.Asp" value="Block" />
                    <rule type="Dxw.Throttling.Asp.Rules.AspRuleOrNode, Dxw.Throttling.Asp">
                        <rules>
                            <rule type="Dxw.Throttling.Asp.Rules.Constant.AspConstantRule, Dxw.Throttling.Asp" value="Pass" />
                            <rule type="Dxw.Throttling.Asp.Rules.Constant.AspConstantRule, Dxw.Throttling.Asp" value="Block" />
                        </rules>    
                    </rule>    
                </rules>
            </rule>
        </rules>
    </throttling>
...
```

And corresponding c# code:
``` cs
...
var config = ConfigurationManager.GetSection(Core.Configuration.Const.DFLT_CONFIG_SECTION_NAME) 
                as ThrottlingConfiguration<IAspArgs, PassBlockVerdict>;

var result = config.Rule.Apply();
Assert.AreEqual(PassBlockVerdict.Block, result.Verdict);
...
```

