using System.Collections.Generic;

namespace day5_hydrothermal_venture
{
    public static class Extensions
    {
        public static IEnumerable<int> EnumerateRange(this (int start, int end) range)
        {
            if (range.start <= range.end)
                for (int i = range.start; i <= range.end; i++)
                    yield return i;
            else
                for (int i = range.start; i >= range.end; i--)
                    yield return i;
        }
    }
}