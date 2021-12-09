using System;
using System.Collections.Generic;

namespace day8_seven_segment_search
{
    public static class Extensions
    {
        public static IEnumerable<char[]> Permute(this char[] pair)
        {
            if (pair.Length != 2)
                throw new ArgumentException();
            yield return pair;
            yield return new[] {pair[1], pair[0]};
        }
    }
}