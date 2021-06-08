using NPOI.SS.UserModel;

namespace Guts.Excel
{
    public class NerpExcelUtils
    {
        public static void SetAllBorder(int fromRow, int toRow, int fromCell, int toCell, INerpExcelBuilder builder)
        {
            SetBorder(fromRow, toRow, fromCell, toCell, builder,
                NerpExcelBorder.Left | NerpExcelBorder.Right | NerpExcelBorder.Top | NerpExcelBorder.Bottom);
        }

        public static void SetBorder(
            int fromRow, int toRow, int fromCell, int toCell, INerpExcelBuilder builder, NerpExcelBorder border, BorderStyle borderStyle = BorderStyle.Thin)
        {
            for (int i = fromRow; i <= toRow; i++)
            {
                for (int j = fromCell; j <= toCell; j++)
                {
                    builder.AtRow(i).AtCell(j).SetBorder(border, borderStyle);
                }
            }
        }
    }
}