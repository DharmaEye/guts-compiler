using System.Collections.Generic;
using System.Linq;

namespace Guts.Patterns.Strategy
{
    public class SkippablePatternRegion : PatternRegion
    {
        private bool _isFirstRun = true;

        public override PatternRegionResult Handle(Token token)
        {
            if (!Commands.Any())
            {
                return Finished; // Finished
            }
            var command = DequeueCommand(token);
            if (command.Type == RegionConsts.EnterChildRegion)
            {
                var handleResult = EnterChildRegion(token);
                if (_isFirstRun && handleResult == PatternRegionResult.Finished)
                {
                    return Finished;
                }
                if (_isFirstRun && handleResult == Failed)
                {
                    return LeaveRegion(token);
                }
                _isFirstRun = true;
                return handleResult;
            }
            if (command.Type == RegionConsts.EnterNeighborRegion)
            {
                var handleResult = EnterNeighborRegion(token);
                if (_isFirstRun && handleResult == PatternRegionResult.Finished)
                {
                    return Finished;
                }
                if (_isFirstRun && handleResult == Failed)
                {
                    return LeaveRegion(token);
                }
                _isFirstRun = true;
                return handleResult;
            }

            if (command.Type == RegionConsts.LeaveRegion)
            {
                return LeaveRegion(token);
            }

            if (command.Type == "*")
            {
                _isFirstRun = false;
                return Succeeded;
            }

            var tokenResult = CheckToken(command.Type, token.Type);
            if (!_isFirstRun || tokenResult == PatternRegionResult.Succeeded)
            {
                return Succeeded;
            }
            Reset();
            return Parent.Handle(token);
        }

        public void Reset()
        {
            Context.SetState(Parent);
            Commands.Clear();
        }

        public SkippablePatternRegion(IReadOnlyDictionary<string, TokenType> keywords, PatternRegionContext context) : base(keywords, context)
        {
        }
    }
}