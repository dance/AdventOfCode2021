using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day6_lanternfish
{
    class Program
    {
        static void Main(string[] args)
        {
            var fishTimers = File.ReadAllLines("input.txt").Single().Split(',').Select(byte.Parse).ToList();
            SolveNaive(fishTimers.ToList(), 80);
            SolveOptimized(fishTimers, 80);
            // SolveNaive(fishTimers, 256); - does not work on such big numbers :)
            SolveOptimized(fishTimers, 256);
        }

        private static void SolveNaive(List<byte> fishTimers, int daysToSimulate)
        {
            for (int day = 1; day <= daysToSimulate; day++)
            for (int i = fishTimers.Count - 1; i >= 0; i--)
            {
                if (fishTimers[i] == 0)
                {
                    fishTimers[i] = 6;
                    fishTimers.Add(8);
                }
                else
                    fishTimers[i]--;
            }
            Console.WriteLine($"Naive solution. Lanternfishes after {daysToSimulate} days: {fishTimers.Count}");
        }
        
        private static void SolveOptimized(List<byte> fishTimers, int daysToSimulate)
        {
            var groupsByDays = fishTimers.GroupBy(t => t).ToDictionary(t => t.Key, t => t.Count());
            ulong[] groupsCounters = new ulong[9];
            for (int i = 0; i < groupsCounters.Length; i++)
                if (groupsByDays.TryGetValue((byte) i, out int count))
                    groupsCounters[i] = (ulong) count;
            for (int day = 1; day <= daysToSimulate; day++)
            {
                ulong resetFishes = groupsCounters[0];
                ulong newFishes = resetFishes;
                
                for (int i = 1; i < groupsCounters.Length; i++)
                {
                    groupsCounters[i - 1] = groupsCounters[i];
                }

                groupsCounters[6] += resetFishes;
                groupsCounters[8] = newFishes;
            }
            ulong totalFishes = groupsCounters.Aggregate(0ul, (aggr, c) => aggr + c);
            Console.WriteLine($"Optimized solution. Lanternfishes after {daysToSimulate} days: {totalFishes}");
        }
    }
}