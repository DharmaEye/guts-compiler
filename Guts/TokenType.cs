using System;

namespace Guts
{
    [Flags]
    public enum TokenType
    {
        None = 0x0,

        // Keywords
        Make = (1 << 2) | TokenTypeFlags.Method,
        Merge     = (1 << 3) | TokenTypeFlags.Method,
        Select    = 1 << 17,
        With      = 1 << 18,
        Row       = 1 << 4,
        Cell      = 1 << 5,
        And       = 1 << 6,
        As        = 1 << 7,
        Set       = 1 << 8,
        Define    = 1 << 19,
        Model     = 1 << 20,
        Variable  = 1 << 25,

        // Literals
        Index            = 1 << 9,
        String           = 1 << 10,
        Number           = 1 << 11,
        Identifier       = 1 << 12,
        Equals           = 1 << 13,

        // Parentheses
        ParenthesesOpen    = 1 << 14,
        ParenthesesClose    = 1 << 15,
        CurlyBracketsOpen  = 1 << 21,
        CurlyBracketsClose = 1 << 22,
        Comma              = 1 << 23,
        Dot                = 1 << 24,
        Next               = 1 << 16
    }
}