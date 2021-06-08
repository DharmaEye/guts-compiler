using NPOI.SS.UserModel;

namespace Guts.Excel
{
    public interface INerpExcelMergeBuilder
    {
        INerpExcelMergeBuilder SetBorder(NerpExcelBorder border, BorderStyle borderStyle = BorderStyle.Thin);
        INerpExcelMergeBuilder SetBorderColor(short color);
        INerpExcelMergeBuilder SetBorderAll(BorderStyle borderStyle = BorderStyle.Thin);
    }
}