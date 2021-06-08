using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Guts.Excel;

namespace Guts
{
    public static class Tokens
    {
        public static IReadOnlyDictionary<string, TokenType> Keywords = new Dictionary<string, TokenType>
        {
            ["row"] = TokenType.Row,
            ["cell"] = TokenType.Cell,
            ["make"] = TokenType.Make,
            ["at"] = TokenType.Index,
            ["merge"] = TokenType.Merge,
            ["with"] = TokenType.With,
            ["number"] = TokenType.Number,
            ["string"] = TokenType.String,
            ["define"] = TokenType.Define,
            ["model"] = TokenType.Model,
            ["identifier"] = TokenType.Identifier,
            ["and"] = TokenType.And,
            ["as"] = TokenType.As,
            ["="] = TokenType.Equals,
            ["'"] = TokenType.String,
            ["("] = TokenType.ParenthesesOpen,
            [")"] = TokenType.ParenthesesClose,
            ["{"] = TokenType.CurlyBracketsOpen,
            ["}"] = TokenType.CurlyBracketsClose,
            [","] = TokenType.Comma,
            ["|"] = TokenType.Next,
            ["."] = TokenType.Dot
        };
    }

    public class Program
    {
        public static void Main()
        {
            var scanner = new Scanner();
            var tokens = scanner.Scan();

            var nerpBuilder = new NerpExcelBuilder();
            var interpreter = new Interpreter();
            var functions = interpreter.Interpret(nerpBuilder, tokens.ToList());
            foreach (var functionalOperation in functions)
            {
                functionalOperation.Invoke();
            }

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "result.xlsx");
            nerpBuilder.Build(path);
        }
    }
}
