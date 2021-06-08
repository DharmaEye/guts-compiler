using System;

namespace Guts.Patterns.Strategy
{
    [Flags]
    public enum PatternRegionType
    {
        Default = 0,
        Iterable = 1 << 1,
        Skippable = 1 << 2
    }
}