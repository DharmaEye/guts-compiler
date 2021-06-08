namespace Guts.Patterns.Strategy
{
    public class PatternRegionContext
    {
        public PatternRegion State { get; private set; }

        public void SetState(PatternRegion state)
        {
            State = state;
        }
    }
}