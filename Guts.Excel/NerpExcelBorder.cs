using System;

namespace Guts.Excel
{
    [Flags]
    public enum NerpExcelBorder
    {
        Top = 0x1 << 0,
        Bottom = 0x1 << 1,
        Right = 0x1 << 2,
        Left = 0x1 << 3
    }
}