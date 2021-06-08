using System.Collections.Generic;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace Guts.Excel
{
    public class NerpExcelBuilder : INerpExcelBuilder
    {
        private readonly XSSFWorkbook _workbook;
        private ISheet _sheet;
        private readonly Dictionary<int, INerpRowBuilder> _rows;

        public NerpExcelBuilder()
        {
            _workbook = new XSSFWorkbook(XSSFWorkbookType.XLSX);
            _rows = new Dictionary<int, INerpRowBuilder>();
        }

        public INerpExcelMergeBuilder Merge(int fromRow, int toRow, int fromCell, int toCell)
        {
            var region = new CellRangeAddress(fromRow, toRow, fromCell, toCell);
            _sheet.AddMergedRegion(region);
            return new NerpExcelMergeBuilder(region, _sheet, this);
        }

        public INerpExcelMergeBuilder Merge(int fromRow, int toRow, int fromCell, int toCell, out int mergeIndex)
        {
            var region = new CellRangeAddress(fromRow, toRow, fromCell, toCell);
            mergeIndex = _sheet.AddMergedRegion(region);
            return new NerpExcelMergeBuilder(region, _sheet, this);
        }

        public INerpExcelBuilder SetWidth(int index, int width)
        {
            _sheet.SetColumnWidth(index, width * 256);
            return this;
        }

        public INerpExcelBuilder CreateSheet(string name)
        {
            _sheet = _workbook.CreateSheet(name);
            return this;
        }

        public INerpExcelBuilder AutoSizeColumn(int index)
        {
            _sheet.AutoSizeColumn(index);
            return this;
        }

        public INerpRowBuilder CreateRow(int index)
        {
            var row = new NerpRowBuilder(_sheet.CreateRow(index), _workbook);
            return row;
        }

        public INerpRowBuilder AtRow(int index)
        {
            if (!_rows.TryGetValue(index, out var row))
            {
                row = new NerpRowBuilder(_sheet.CreateRow(index), _workbook);
                _rows.Add(index, row);
            }
            return row;
        }

        public void Build(string saveDirectory)
        {
            using var ms = new MemoryStream();
            using var fs = new FileStream(saveDirectory, FileMode.Create, FileAccess.Write);
            _workbook.Write(fs);
        }

        public INerpExcelMergeBuilder AtMerge(int index)
        {
            var mergedRegion = _sheet.GetMergedRegion(index);
            return new NerpExcelMergeBuilder(mergedRegion, _sheet, this);
        }
    }
}