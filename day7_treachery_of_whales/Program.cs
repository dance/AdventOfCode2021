using System;
using System.IO;
using System.Linq;
using static System.Math;

namespace day7_treachery_of_whales
{
    class Program
    {
        static void Main(string[] args)
        {
            Part1();
            Part2();
            TestPart2();
        }

        private static void Part1()
        {
            var input = File.ReadAllLines("input.txt").Single().Split(',').Select(int.Parse).ToArray();
            var posCosts = new int[input.Length];
            for (int i = 0; i < input.Length; i++)
            for (int j = 0; j < input.Length; j++)
                posCosts[i] += Abs(input[i] - input[j]);

            Console.WriteLine($"Part 1. Fuel needed: {posCosts.Min()}");
        }
        
        private static void Part2()
        {
            var input = File.ReadAllLines("input.txt").Single().Split(',').Select(int.Parse).ToArray();
            SolvePart2(input);
        }

        private static void SolvePart2(int[] input)
        {
            var posCosts = new int[input.Length];
            
            for (int i = 0; i < input.Length; i++)
            for (int j = 0; j < input.Length; j++)
                posCosts[i] += GetPathCost(i, input[j]);
            
            Console.WriteLine($"Part 2. Fuel needed: {posCosts.Min()}");
        }

        private static int GetPathCost(int from, int to)
        {
            int cost = 0;
            int step = 1;
            for (int i = Min(from, to); i < Max(from, to); i++) 
                cost += step++;

            return cost;
        }

        private static void TestPart2()
        {
            Console.WriteLine("Part 2 test:");
            SolvePart2(new []{ 16,1,2,0,4,2,7,1,2,14 });
        }
    }
}