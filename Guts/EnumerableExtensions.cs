using System.Collections.Generic;

namespace Guts
{
    public static class EnumerableExtensions
    {
        public static Queue<T> ToQueue<T>(this IEnumerable<T> ts)
        {
            return new Queue<T>(ts);
        }
    }
}