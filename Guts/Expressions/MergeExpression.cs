using Guts.Operations;

namespace Guts.Expressions
{
    public class MergeExpression : Expression
    {
        public override FunctionalOperation Interpret(LogicContext context)
        {
            var pattern = "merge *[rowA] *[cellA] with *[rowB] *[cellB]";
            return CreateByPattern<MergeFunctionalOperation>(pattern, context);
        }
    }
}