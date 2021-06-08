using Guts.Excel;
using Guts.Patterns;

namespace Guts.Operations
{
    public class MergeFunctionalOperation : FunctionalOperation
    {
        public MergeFunctionalOperation(FunctionalOperationStatus status, StateMachine state, INerpExcelBuilder builder, PatternMatchResult matchResult) 
            : base("Merge", status, state, builder, matchResult)
        {
        }

        public MergeFunctionalOperation(FunctionalOperationStatus status) : base(status)
        {
        }

        public override FunctionalOperationInvokeResult Invoke()
        {
            var rowA = State.GetVariable(GetAttribute("rowA").Object.ToString()).GetAttribute("index").Object.ToInt();
            var cellA = State.GetVariable(GetAttribute("cellA").Object.ToString()).GetAttribute("index").Object.ToInt();

            var rowB = State.GetVariable(GetAttribute("rowB").Object.ToString()).GetAttribute("index").Object.ToInt();
            var cellB = State.GetVariable(GetAttribute("cellB").Object.ToString()).GetAttribute("index").Object.ToInt();

            if (rowA > rowB)
            {
                rowA ^= rowB;
                rowB ^= rowA;
                rowA ^= rowB;
            }

            if (cellA > cellB)
            {
                cellA ^= cellB;
                cellB ^= cellA;
                cellA ^= cellB;
            }

            Builder.Merge(rowA, rowB, cellA, cellB);
            return base.Invoke();
        }
    }
}