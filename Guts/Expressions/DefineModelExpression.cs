using Guts.CustomReaders;
using Guts.Operations;

namespace Guts.Expressions
{
    public class DefineModelExpression : Expression
    {
        public override FunctionalOperation Interpret(LogicContext context)
        {
            var pattern = "define model as *[value]";
            var functionalOperation = CreateByPattern<DefineModelFunctionalOperation>(pattern, context);
            if (functionalOperation.Status == FunctionalOperationStatus.Failed)
            {
                return functionalOperation;
            }
            var structureReader = new StructureReader(context.Tokens);
            var structure = structureReader.ReadStructure();
            functionalOperation.AddAttribute("Structure", structure);
            return functionalOperation;
        }
    }
}