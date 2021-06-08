using System;
using NPOI.SS.UserModel;

namespace Guts.Excel
{
    public class NerpCellBuilder : INerpCellBuilder
    {
        private readonly ICell _cell;
        private readonly ICellStyle _cellStyle;
        private readonly IFont _font;

        public int Index { get; set; }

        public NerpCellBuilder(ICell cell, IWorkbook workbook)
        {
            _cell = cell;
            _cellStyle = workbook.CreateCellStyle();
            _font = workbook.CreateFont();
            _font.FontName = "Sylfaen";
            _cell.CellStyle = _cellStyle;
            _cell.CellStyle.SetFont(_font);
        }

        public INerpCellBuilder SetBorderAll(BorderStyle borderStyle)
        {
            SetBorder(NerpExcelBorder.Bottom | NerpExcelBorder.Top | NerpExcelBorder.Left | NerpExcelBorder.Right, borderStyle);
            return this;
        }

        public INerpCellBuilder SetFontSize(short fontSize)
        {
            _font.FontHeightInPoints = fontSize;
            return this;
        }

        public INerpCellBuilder SetHorizontalTextAlign(HorizontalAlignment align)
        {
            _cellStyle.Alignment = align;
            return this;
        }

        public INerpCellBuilder SetVerticalTextAlign(VerticalAlignment align)
        {
            _cellStyle.VerticalAlignment = align;
            return this;
        }

        public INerpCellBuilder SetBorder(NerpExcelBorder border, BorderStyle borderStyle = BorderStyle.Thin)
        {
            if (border.HasFlag(NerpExcelBorder.Bottom))
            {
                _cellStyle.BorderBottom = borderStyle;
            }
            if (border.HasFlag(NerpExcelBorder.Left))
            {
                _cellStyle.BorderLeft = borderStyle;
            }
            if (border.HasFlag(NerpExcelBorder.Right))
            {
                _cellStyle.BorderRight = borderStyle;
            }
            if (border.HasFlag(NerpExcelBorder.Top))
            {
                _cellStyle.BorderTop = borderStyle;
            }
            return this;
        }

        public INerpCellBuilder SetFontName(string name)
        {
            _font.FontName = name;
            return this;
        }

        public INerpCellBuilder SetBorderColor(short color)
        {
            _cellStyle.BottomBorderColor = color;
            _cellStyle.TopBorderColor = color;
            _cellStyle.LeftBorderColor = color;
            _cellStyle.RightBorderColor = color;
            return this;
        }

        public INerpCellBuilder SetFontWeight(NerpFontWeight weight)
        {
            _font.IsBold = weight switch
            {
                NerpFontWeight.Normal => false,
                NerpFontWeight.Bold => true,
                _ => throw new ArgumentOutOfRangeException(nameof(weight), weight, null)
            };
            return this;
        }

        public INerpCellBuilder SetColor(short color)
        {
            _cellStyle.FillForegroundColor = color;
            //_cellStyle.FillPattern = FillPattern.SolidForeground;
            return this;
        }

        public INerpCellBuilder SetBackgroundColor(short color)
        {
            _cellStyle.FillForegroundColor = color;
            _cellStyle.FillPattern = FillPattern.SolidForeground;
            return this;
        }

        public INerpCellBuilder RotateUp()
        {
            _cellStyle.Rotation = 90;
            return this;
        }


        public INerpCellBuilder SetText(string value)
        {
            _cell.SetCellValue(value);
            return this;
        }

        public INerpCellBuilder SetText(int value)
        {
            _cell.SetCellValue(value);
            return this;
        }

        public INerpCellBuilder SetText(bool value)
        {
            _cell.SetCellValue(value);
            return this;
        }

        public INerpCellBuilder WrapText()
        {
            _cellStyle.WrapText = true;
            return this;
        }

        public INerpCellBuilder ShrinkToFit()
        {
            _cellStyle.ShrinkToFit = true;
            return this;
        }
    }
}