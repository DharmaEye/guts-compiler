using Guts.Excel;
using Guts.Patterns;

namespace Guts.Operations
{
    public class CellFunctionalOperation : FunctionalOperation
    {
        private INerpCellBuilder _cell;

        public CellFunctionalOperation(
            FunctionalOperationStatus status,
            StateMachine state,
            INerpExcelBuilder builder,
            PatternMatchResult result)
            : base("cell", status, state, builder, result)
        {
        }

        public CellFunctionalOperation(FunctionalOperationStatus status) : base(status)
        {
        }

        public override FunctionalOperationInvokeResult Invoke()
        {
            if (MatchResult.TryGetVariable("name", out var variable))
            {
                var name = variable.GetFirst();
                State.AddVariable(name.ToString(), this);
            }

            var rowIndex = int.Parse(Root.GetAttribute("Index").Object.ToString() ?? string.Empty);
            var cellIndex = int.Parse(GetAttribute("Index").Object.ToString() ?? string.Empty);
            _cell = Builder.AtRow(rowIndex).AtCell(cellIndex);
            return base.Invoke();
        }

        public override void OnSetProperty(string property, object value)
        {
            switch (property.ToLower())
            {
                case "text":
                    _cell.SetText(value.ToString());
                    break;
                case "fontsize":
                    if (!short.TryParse(value.ToString(), out var fontSize))
                    {
                        throw new RuntimeException("Font size is incorrect");
                    }
                    _cell.SetFontSize(fontSize);
                    return;
                case "fontweight":
                    var fontWeight = value.ToString()?.ToLower();
                    _cell.SetFontWeight(fontWeight == "bold" ? NerpFontWeight.Bold : NerpFontWeight.Normal);
                    return;
                default:
                    Parent.OnSetProperty(property, value);
                    return;
            }
        }
    }
}