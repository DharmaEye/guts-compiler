using Guts.Operations;

namespace Guts.Expressions
{
    public class MakeCellExpression : Expression
    {
        public override FunctionalOperation Interpret(LogicContext context)
        {
            var pattern = "make cell ?as *[name]? at *[index]";
            return CreateByPattern<CellFunctionalOperation>(pattern, context);
        }
    }
}