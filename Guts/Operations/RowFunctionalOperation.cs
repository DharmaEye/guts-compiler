using Guts.Excel;
using Guts.Patterns;

namespace Guts.Operations
{
    public class RowFunctionalOperation : FunctionalOperation
    {
        private INerpRowBuilder _row;

        public RowFunctionalOperation(
            FunctionalOperationStatus status,
            StateMachine state,
            INerpExcelBuilder builder,
            PatternMatchResult result)
            : base("Row function", status, state, builder, result)
        {
        }

        public RowFunctionalOperation(FunctionalOperationStatus status) : base(status)
        {

        }

        public override FunctionalOperationInvokeResult Invoke()
        {
            if (MatchResult.TryGetVariable("name", out var variable))
            {
                State.AddVariable(variable.GetFirst().ToString(), this);
            }
            _row = Builder.AtRow(int.Parse(GetAttribute("Index").Object.ToString() ?? string.Empty));
            return base.Invoke();
        }

        public override void OnSetProperty(string property, object value)
        {
            switch (property.ToLower())
            {
                case "height":
                    if (!int.TryParse(value.ToString(), out var height))
                    {
                        throw new RuntimeException(property + " is invalid");
                    }
                    _row.SetHeight(height);
                    return;
                default:
                    return;
            }
        }
    }
}