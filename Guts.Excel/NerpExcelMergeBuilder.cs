using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Guts.Excel
{
    public class NerpExcelMergeBuilder : INerpExcelMergeBuilder
    {
        private readonly ISheet _sheet;
        private readonly INerpExcelBuilder _builder;
        private readonly CellRangeAddress _region;

        public NerpExcelMergeBuilder(CellRangeAddress region, ISheet sheet, INerpExcelBuilder builder)
        {
            _sheet = sheet;
            _builder = builder;
            _region = region;
        }

        public INerpExcelMergeBuilder SetBorder(NerpExcelBorder border, BorderStyle borderStyle)
        {
            if (border.HasFlag(NerpExcelBorder.Bottom))
            {
                NerpExcelUtils.SetBorder(
                    _region.FirstRow,
                    _region.LastRow,
                    _region.FirstColumn,
                    _region.LastColumn,
                    _builder,
                    NerpExcelBorder.Bottom,
                    borderStyle
                );
            }
            if (border.HasFlag(NerpExcelBorder.Left))
            {
                NerpExcelUtils.SetBorder(
                    _region.FirstRow,
                    _region.LastRow,
                    _region.FirstColumn,
                    _region.LastColumn,
                    _builder,
                    NerpExcelBorder.Left,
                    borderStyle
                );
            }
            if (border.HasFlag(NerpExcelBorder.Right))
            {
                NerpExcelUtils.SetBorder(
                    _region.FirstRow,
                    _region.LastRow,
                    _region.FirstColumn,
                    _region.LastColumn,
                    _builder,
                    NerpExcelBorder.Right,
                    borderStyle
                );
            }
            if (border.HasFlag(NerpExcelBorder.Top))
            {
                NerpExcelUtils.SetBorder(
                    _region.FirstRow,
                    _region.LastRow,
                    _region.FirstColumn,
                    _region.LastColumn,
                    _builder,
                    NerpExcelBorder.Top,
                    borderStyle
                );
            }
            return this;
        }

        public INerpExcelMergeBuilder SetBorderColor(short color)
        {
            RegionUtil.SetBottomBorderColor(color, _region, _sheet);
            RegionUtil.SetTopBorderColor(color, _region, _sheet);
            RegionUtil.SetLeftBorderColor(color, _region, _sheet);
            RegionUtil.SetRightBorderColor(color, _region, _sheet);
            return this;
        }

        public INerpExcelMergeBuilder SetBorderAll(BorderStyle borderStyle = BorderStyle.Thin)
        {
            SetBorder(NerpExcelBorder.Bottom | NerpExcelBorder.Left | NerpExcelBorder.Right | NerpExcelBorder.Top, borderStyle);
            return this;
        }
    }
}