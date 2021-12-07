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
            SolvePart1WithMedian();
        }

        private static void Part1()
        {
            var input = File.ReadAllLines("input.txt").Single().Split(',').Select(int.Parse).ToArray();
            var posCosts = new int[input.Length];
            for (int i = 0; i < input.Length; i++)
            for (int j = 0; j < input.Length; j++)
                posCosts[i] += Abs(input[i] - input[j]); //a BUG here - must be just 'i' instead of 'input[i]' but it still works

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
                posCosts[i] += GetPathCost(i, input[j]); // that bug revealed up here, took some time to figure it out :(
            
            Console.WriteLine($"Part 2. Fuel needed: {posCosts.Min()}");
        }

        private static int GetPathCost(int from, int to)
        {
            int cost = 0;
            // that's embarrassing, I know
            // int step = 1;
            // for (int i = Min(from, to); i < Max(from, to); i++) 
            //     cost += step++;
            // let's count as smart people do
            int steps = Abs(from - to);
            cost = (1 + steps) * steps / 2;

            return cost;
        }

        private static void TestPart2()
        {
            Console.WriteLine("Part 2 test:");
            SolvePart2(new []{ 16,1,2,0,4,2,7,1,2,14 });
            Console.WriteLine("Expected 168");
        }

        private static void SolvePart1WithMedian()
        {
            var input = File.ReadAllLines("input.txt").Single().Split(',').Select(int.Parse).ToArray();
            int median = Quickselect.QuickselectMedian(input);
            int fuelCost = 0;
            for (int i = 0; i < input.Length; i++)
                fuelCost += Abs(median - input[i]);
            Array.Sort(input);
            Console.WriteLine($"Part 1. Median. Quickselect median = {median}, middle of sorted array is {input[input.Length / 2]}");
            Console.WriteLine($"Part 1. Median. Fuel costs: {fuelCost}");
        }
    }
}