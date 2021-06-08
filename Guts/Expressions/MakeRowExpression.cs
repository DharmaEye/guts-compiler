using Guts.Operations;

namespace Guts.Expressions
{
    public class MakeRowExpression : Expression
    {
        public override FunctionalOperation Interpret(LogicContext context)
        {
            var pattern = "make row ?as *[name]? at *[index]";
            return CreateByPattern<RowFunctionalOperation>(pattern, context);
        }
    }
}