using Guts.CustomReaders;
using Guts.Excel;
using Guts.Patterns;

namespace Guts.Operations
{
    public class DefineModelFunctionalOperation : FunctionalOperation
    {
        public DefineModelFunctionalOperation(
            FunctionalOperationStatus status,
            StateMachine state,
            INerpExcelBuilder builder,
            PatternMatchResult matchResult) : base("DefineModel", status, state, builder, matchResult)
        {
        }

        public DefineModelFunctionalOperation(FunctionalOperationStatus status) : base(status)
        {
        }

        public override FunctionalOperationInvokeResult Invoke()
        {
            State.AddStructure(
                GetAttribute("value").Object.ToString(),
                GetAttribute("structure").Object as Structure
            );
            return base.Invoke();
        }
    }
}