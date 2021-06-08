using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Guts
{
    public class Scanner
    {
        private readonly string _source;
        private int _currentLine = 1;
        private int _current;
        private int _start;

        private readonly IReadOnlyDictionary<string, TokenType> _keywords;
        private readonly ICollection<Token> _tokens;

        public Scanner()
        {
            _tokens = new List<Token>();
            _keywords = Tokens.Keywords;
            _source = File.ReadAllText(@"test.txt");
        }

        private void ClearVoids()
        {
            while (CanKeepReading() && (Peek() == ' ' || Peek() == '\t' || Peek() == '\n' || Peek() == '\r'))
            {
                _current++;
                _start = _current;
            }
        }

        private bool CanKeepReading()
        {
            return _current < _source.Length;
        }

        public IEnumerable<Token> Scan()
        {
            while (CanKeepReading())
            {
                ClearVoids();

                switch (Peek())
                {
                    case '(':
                        AddToken(new Token(TokenType.ParenthesesOpen, _currentLine, Peek()));
                        break;
                    case ')':
                        AddToken(new Token(TokenType.ParenthesesClose, _currentLine, Peek()));
                        break;
                    case '=':
                        AddToken(new Token(TokenType.Equals, _currentLine, Peek()));
                        break;
                    case '{':
                        AddToken(new Token(TokenType.CurlyBracketsOpen, _currentLine, Peek()));
                        break;
                    case '}':
                        AddToken(new Token(TokenType.CurlyBracketsClose, _currentLine, Peek()));
                        break;
                    case ',':
                        AddToken(new Token(TokenType.Comma, _currentLine, Peek()));
                        break;
                    case '|':
                        AddToken(new Token(TokenType.Next, _currentLine, Peek()));
                        break;
                    case '\'':
                        var @string = ReadString(_current);
                        AddToken(new Token(TokenType.String, _currentLine, @string.Substring(1, @string.Length - 2)));
                        break;
                }

                if (IsAlphabet(Peek()))
                {
                    var keyword = Keyword(_current);
                    if (CheckToken(keyword))
                    {
                        AddToken(new Token(_keywords[keyword], _currentLine, keyword));
                    }
                    else
                    {
                        AddToken(new Token(TokenType.Identifier, _currentLine, keyword));
                    }
                }

                if (IsDigit(Peek()) || Peek() == '.' && IsDigit(PeekNext()))
                {
                    var number = Number();
                    AddToken(new Token(TokenType.Number, _currentLine, number));
                }

                if (Peek() == '=' && TryGetVariable(out var variable))
                {
                    AddToken(new Token(TokenType.Variable, _currentLine, variable));
                }
                _current++;
            }

            return _tokens;
        }

        public bool TryGetVariable(out string variable)
        {
            variable = string.Empty;
            var current = _current;
            while (Peek() == '=')
            {
                _current++;
            }
            ClearVoids();
            if (!IsAlphabet(Peek()))
            {
                _current = current;
                return false;
            }
            var nestedVariables = new List<string>();
            while (IsAlphabet(Peek()))
            {
                var keyword = Keyword(_current);
                nestedVariables.Add(keyword);
                if (PeekNext() != '.')
                {
                    break;
                }
                _current += 2;
            }
            variable = string.Join('.', nestedVariables);
            return true;
        }

        public char Peek()
        {
            if (_current >= _source.Length)
            {
                return '\0';
            }
            return _source[_current];
        }

        public char PeekNext()
        {
            if (_current + 1 >= _source.Length)
            {
                return '\0';
            }
            return _source[_current + 1];
        }

        public bool IsAlphabet(char c)
        {
            return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z';
        }

        public bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        public string ReadString(int from)
        {
            _current++;
            while (Peek() != '\'')
            {
                _current++;
            }
            //_current++;
            //_current++;
            return Substr(from);
        }

        public string Substr(int from)
        {
            return _source.Substring(from, (_current - from) + 1);
        }

        public string Keyword(int from)
        {
            while (IsAlphabet(PeekNext()))
            {
                _current++;
            }
            return Substr(from);
        }

        public string Keyword()
        {
            return Keyword(_start);
        }

        public string Number(int from)
        {
            while (IsDigit(PeekNext()) || PeekNext() == '.')
            {
                _current++;
            }
            return Substr(from);
        }

        public string Number()
        {
            return Number(_start);
        }

        public void AddToken(Token token)
        {
            _tokens.Add(token);
        }

        public bool CheckToken(string token)
        {
            return _keywords.TryGetValue(token, out var type);
        }
    }
}