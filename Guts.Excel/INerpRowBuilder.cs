namespace Guts.Excel
{
    public interface INerpRowBuilder
    {
        /// <summary>
        /// Select cell for styling
        /// </summary>
        /// <returns></returns>
        INerpCellBuilder AtCell(int index);

        INerpCellBuilder CreateCell(int index);

        INerpRowBuilder SetHeight(int height);
    }
}