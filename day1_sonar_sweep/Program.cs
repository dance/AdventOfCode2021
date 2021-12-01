using System;
using System.IO;
using System.Linq;

namespace day1_sonar_sweep
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt").Select(int.Parse).ToArray();
            int previousDepth = input[0];
            int depthIncreasedCount = 0;
            for (int i = 1; i < input.Length; i++)
            {
                if (input[i] > previousDepth)
                    depthIncreasedCount++;
                previousDepth = input[i];
            }
            Console.WriteLine($"Depth increased times: {depthIncreasedCount}");
        }
    }
}