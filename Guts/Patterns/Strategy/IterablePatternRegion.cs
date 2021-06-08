using System.Collections.Generic;
using System.Linq;

namespace Guts.Patterns.Strategy
{
    public class IterablePatternRegion : PatternRegion
    {
        public override PatternRegionResult Handle(Token token)
        {
            if (!Commands.Any())
            {
                if (Parent != null)
                {
                    Context.SetState(Parent);
                }
                return Finished;
            }
            var command = DequeueCommand(token);
            if (command.Type == RegionConsts.EnterChildRegion)
            {
                return EnterChildRegion(token);
            }
            if (command.Type == RegionConsts.EnterNeighborRegion)
            {
                return EnterChildRegion(token);
            }
            if (command.Type == RegionConsts.LeaveRegion)
            {
                return LeaveRegion(token);
            }
            if (command.Type == "*")
            {
                if (command.HasVariable)
                {
                    command.SetVariableValue(token.Literal);
                }
                return Succeeded;
            }
            var tokenResult = CheckToken(command.Type, token.Type);
            if (command.IsSeparator)
            {
                Commands.Clear();
                if (tokenResult == Failed)
                {
                    Context.SetState(Parent);
                    return Parent.Handle(token);
                }

                RestoreSubStates();
                return Succeeded;
            }
            return tokenResult;
        }

        public IterablePatternRegion(IReadOnlyDictionary<string, TokenType> keywords, PatternRegionContext context) : base(keywords, context)
        {
        }
    }
}