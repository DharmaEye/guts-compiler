using System.Collections.Generic;
using NPOI.SS.UserModel;

namespace Guts.Excel
{
    public class NerpRowBuilder : INerpRowBuilder
    {
        private readonly IRow _row;
        private readonly IWorkbook _workbook;
        private readonly IDictionary<int, INerpCellBuilder> _cells;

        public NerpRowBuilder(IRow row, IWorkbook workbook)
        {
            _row = row;
            _workbook = workbook;
            _cells = new Dictionary<int, INerpCellBuilder>();
        }

        public INerpCellBuilder AtCell(int index)
        {
            if (!_cells.TryGetValue(index, out var cell))
            {
                cell = new NerpCellBuilder(_row.CreateCell(index), _workbook);
                _cells.Add(index, cell);
            }
            return cell;
        }

        public INerpCellBuilder CreateCell(int index)
        {
            var cell = new NerpCellBuilder(_row.CreateCell(index), _workbook);
            _cells.Add(index, cell);
            return cell;
        }

        public INerpRowBuilder SetHeight(int height)
        {
            _row.HeightInPoints = height;
            return this;
        }
    }
}