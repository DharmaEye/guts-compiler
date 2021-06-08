using System.Collections.Generic;
using System.Linq;

namespace Guts.Patterns.Strategy
{
    public class DefaultPatternRegion : PatternRegion
    {
        public override PatternRegionResult Handle(Token token)
        {
            if (!Commands.Any())
            {
                return Finished; // Finished
            }
            var command = DequeueCommand(token);
            if (command.Type == RegionConsts.EnterChildRegion)
            {
                return EnterChildRegion(token);
            }
            if (command.Type == RegionConsts.EnterNeighborRegion)
            {
                return EnterNeighborRegion(token);
            }
            if (command.Type == "*")
            {
                return Succeeded;
            }
            return CheckToken(command.Type, token.Type);
        }

        public DefaultPatternRegion(IReadOnlyDictionary<string, TokenType> keywords, PatternRegionContext context) : base(keywords, context)
        {
        }
    }
}