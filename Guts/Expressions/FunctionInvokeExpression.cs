using Guts.Operations;

namespace Guts.Expressions
{
    public class FunctionInvokeExpression : Expression
    {
        public override FunctionalOperation Interpret(LogicContext context)
        {
            var pattern = "*(?^Identifier[name] = *[value]>|^?)";
            return CreateByPattern<MethodFunctionalOperation>(pattern, context);
        }
    }
}