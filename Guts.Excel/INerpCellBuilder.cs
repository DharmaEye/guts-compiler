using NPOI.SS.UserModel;

namespace Guts.Excel
{
    public interface INerpCellBuilder
    {
        int Index { get; set; }

        INerpCellBuilder SetFontSize(short fontSize);
        INerpCellBuilder SetHorizontalTextAlign(HorizontalAlignment align);
        INerpCellBuilder SetBorder(NerpExcelBorder border, BorderStyle borderStyle = BorderStyle.Thin);
        /// <summary>
        /// Usage example: HSSFColor.Blue.Index
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        INerpCellBuilder SetBorderColor(short color);
        /// <summary>
        /// Usage example: HSSFColor.Blue.Index
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        INerpCellBuilder SetColor(short color);

        INerpCellBuilder SetVerticalTextAlign(VerticalAlignment align);
        INerpCellBuilder SetBackgroundColor(short color);
        INerpCellBuilder SetText(string value);
        INerpCellBuilder SetText(int value);
        INerpCellBuilder SetText(bool value);
        INerpCellBuilder SetFontWeight(NerpFontWeight weight);
        INerpCellBuilder WrapText();
        INerpCellBuilder ShrinkToFit();
        INerpCellBuilder SetFontName(string name);
        INerpCellBuilder RotateUp();
        INerpCellBuilder SetBorderAll(BorderStyle borderStyle = BorderStyle.Thin);
    }
}