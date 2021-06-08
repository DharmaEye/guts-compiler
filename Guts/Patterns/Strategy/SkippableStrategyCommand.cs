namespace Guts.Patterns.Strategy
{
    public class SkippableStrategyCommand : StrategyCommand
    {
        public SkippableStrategyCommand(string type) : base(type)
        {
        }
    }
}