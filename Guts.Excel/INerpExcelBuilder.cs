namespace Guts.Excel
{
    public interface INerpExcelBuilder
    {
        INerpExcelBuilder CreateSheet(string name);
        INerpExcelMergeBuilder Merge(int fromRow, int toRow, int fromCell, int toCell);
        INerpExcelBuilder SetWidth(int index, int width);
        INerpRowBuilder CreateRow(int index);
        INerpRowBuilder AtRow(int index);
        void Build(string saveDirectory);
        INerpExcelMergeBuilder AtMerge(int index);
        INerpExcelBuilder AutoSizeColumn(int index);
    }
}