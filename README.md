# Guts

## Little summary

Guts is a small compiler which I've made for fun. It supposed to be an easy tool for excel generation but it's hasn't
been finished yet. I've made it couple months ago but hasn't used for anything and in the end I've decided to publish
it. I hope it will help you to learn something new.

*If you think that something could be done in a different way please let me know. Thanks in advance ^_^*

## System

I've tried to create a declarative approach rather than an imperative.

In the beginning `Scanner` tries to parse the text code and collect the language words called Token.

Once we've got the tokens, it's being matched and translated into the `FunctionalOperation`.

Expressions are used to give meaning to a group of tokens.

For an example

```c#
public class MakeCellExpression : Expression
{
    public override FunctionalOperation Interpret(LogicContext context)
    {
        var pattern = "make cell ?as *[name]? at *[index]";
        return CreateByPattern<CellFunctionalOperation>(pattern, context);
    }
}
```

As you can see in the above example we've created an expression that creates `FunctionalOperation` if the pattern matches.

`PatternProvider` verifies patterns. In the above example the pattern contains symbols: {?, *, []}.
Each symbol has meaning.

For an example:
```
? - optional token
*[] - variable name
^Sth>|^ - multiple values (>| separated by) - e.g: One | Two | Three
```

`Interpreter` holds the expressions, it has `Interpret()` method which receives the tokens and iterates over the expressions.
If an expression matches it removes a group of the tokens which were used to match the pattern and tries to match the other tokens.
Each expression iteration is being counted, if the counted expressions are the same after iterate it throws `RuntimeException` (No matching expression found).

**More coming soon...**