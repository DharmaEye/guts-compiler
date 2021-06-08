using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Guts.Patterns.Strategy;

namespace Guts.Patterns
{
    public class PatternProvider
    {
        private readonly ICollection<Token> _tokens;

        public PatternProvider(ICollection<Token> tokens, string pattern)
        {
            _tokens = tokens;
            _strategy = new MatchStrategy(pattern, Tokens.Keywords);
        }

        private readonly MatchStrategy _strategy;
        private PatternRegionResult ReadCommand(Token token)
        {
            return _strategy.ReadCommand(token);
        }

        public PatternMatchResult Match()
        {
            var proceededTokens = new List<Token>();
            Action removeProceededTokens = () =>
            {
                foreach (var proceededToken in proceededTokens)
                {
                    _tokens.Remove(proceededToken);
                }
            };

            foreach (var token in _tokens)
            {
                var readCommandResult = ReadCommand(token);
                if (readCommandResult == PatternRegionResult.Failed)
                {
                    return new PatternMatchResult(false);
                }


                if (readCommandResult == PatternRegionResult.Finished)
                {
                    removeProceededTokens();
                    return new PatternMatchResult(proceededTokens.Last().Type == TokenType.And, true, _strategy.GetVariables());
                }
                proceededTokens.Add(token);
            }

            if (!_strategy.IsFinished())
            {
                return new PatternMatchResult(false);
            }

            removeProceededTokens();
            return new PatternMatchResult(proceededTokens.Last().Type == TokenType.And, true, _strategy.GetVariables());
        }
    }
}