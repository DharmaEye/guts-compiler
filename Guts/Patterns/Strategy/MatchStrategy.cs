using System;
using System.Collections.Generic;

namespace Guts.Patterns.Strategy
{
    public class MatchStrategy
    {
        private readonly string _pattern;
        private readonly IReadOnlyDictionary<string, TokenType> _keywords;
        private int _start;
        private int _current;

        private PatternRegionType _patternRegion = PatternRegionType.Default;
        private PatternRegionType _prevPatternRegion = PatternRegionType.Default;

        private readonly PatternRegionContext _context;

        public MatchStrategy(string pattern, IReadOnlyDictionary<string, TokenType> keywords)
        {
            _pattern = pattern;
            _keywords = keywords;
            _context = new PatternRegionContext();
            _mainRegion = new DefaultPatternRegion(keywords, _context);
            _context.SetState(_mainRegion);
            _curr = _mainRegion;
            ParseCommands();
        }

        private readonly PatternRegion _mainRegion;
        private PatternRegion _curr;

        private void RegionModified(PatternRegionType modifiedRegion)
        {
            if (_patternRegion != _prevPatternRegion)
            {
                // Leave
                if (_prevPatternRegion > _patternRegion)
                {
                    EnqueueCommand(new StrategyCommand(RegionConsts.LeaveRegion));
                    _curr = _curr.Parent;
                }

                // Enter
                if (_patternRegion > _prevPatternRegion)
                {
                    var region = CreateRegion(modifiedRegion);
                    _curr.AddChild(region);
                    _curr = region;
                }
            }
            _prevPatternRegion = _patternRegion;
        }

        public void ParseCommands()
        {
            while (!IsAtEnd())
            {
                SkipWhitespaces();
                if (Peek() == '^')
                {
                    _patternRegion ^= PatternRegionType.Iterable;
                    RegionModified(PatternRegionType.Iterable);
                    Advance();
                    continue;
                }
                if (Peek() == '?')
                {
                    _patternRegion ^= PatternRegionType.Skippable;
                    RegionModified(PatternRegionType.Skippable);
                    Advance();
                    continue;
                }

                if (Peek() == '[')
                {
                    var variable = ReadVariable();
                    _curr.SetVariable(variable);
                    continue;
                }

                if (IsAlphabet(Peek()))
                {
                    _start = _current;
                    var current = ReadString().Trim().ToLower();
                    EnqueueCommand(CreateCommand(current));
                }
                else
                {
                    EnqueueCommand(CreateCommand(Peek().ToString()));
                }

                Advance();
            }
        }

        public bool IsFinished()
        {
            return _mainRegion.IsFinished();
        }

        private PatternRegion CreateRegion(PatternRegionType type)
        {
            PatternRegion handler = type switch
            {
                PatternRegionType.Default => new DefaultPatternRegion(_keywords, _context),
                PatternRegionType.Iterable => new IterablePatternRegion(_keywords, _context),
                PatternRegionType.Skippable => new SkippablePatternRegion(_keywords, _context),
                _ => throw new ArgumentOutOfRangeException()
            };
            return handler;
        }

        private void EnqueueCommand(StrategyCommand command)
        {
            _curr.EnqueueCommand(command);
        }

        private StrategyCommand CreateCommand(string type)
        {
            if (_patternRegion.HasFlag(PatternRegionType.Iterable))
            {
                var isSeparator = false;
                if (Peek() == '>')
                {
                    Advance();
                    type = Peek().ToString();
                    isSeparator = true;
                }

                var command = new StrategyCommand(type);
                command.SetSeparator(isSeparator);
                return command;
            }

            return new StrategyCommand(type);
        }

        public bool IsAtEnd()
        {
            return _current > _pattern.Length - 1;
        }

        public PatternRegionResult ReadCommand(Token token)
        {
            var handleResult = _context.State.Handle(token);
            return handleResult;
        }

        private string ReadVariable()
        {
            _start = _current;
            while (!IsAtEnd())
            {
                if (Peek() == ']')
                {
                    Advance();
                    return _pattern.Substring(_start, (_current - _start));
                }
                Advance();
            }

            throw new RuntimeException("Cannot read pattern variable");
        }

        private void SkipWhitespaces()
        {
            while (!IsAtEnd() && (Peek() == '\t' || Peek() == '\n' || Peek() == ' '))
            {
                Advance();
                _start = _current;
            }
        }

        public char Peek()
        {
            if (IsAtEnd())
            {
                return '\0';
            }

            return _pattern[_current];
        }

        public void Advance()
        {
            _current++;
        }

        public bool IsAlphabet(char c)
        {
            return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c == '\'';
        }

        public char PeekNext()
        {
            if (_current + 1 >= _pattern.Length)
            {
                return '\0';
            }
            return _pattern[_current + 1];
        }

        private string ReadString()
        {
            while (!IsAtEnd() && IsAlphabet(PeekNext()))
            {
                Advance();
            }

            return _pattern.Substring(_start, (_current - _start) + 1);
        }

        public Dictionary<string, StateVariable> GetVariables()
        {
            var contextState = _mainRegion.State;
            while (contextState.Parent != null)
            {
                contextState = contextState.Parent;
            }
            return contextState.GetVariables();
        }
    }
}