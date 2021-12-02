using System;
using System.IO;

namespace day2_dive
{
    class Program
    {
        static void Main(string[] args)
        {
            Part1();
            Part2();
        }

        static void Part1()
        {
            var input = File.ReadAllLines("input.txt");

            int distance = 0;
            int depth = 0;

            foreach (string line in input)
            {
                string[] split = line.Split(' ');
                string direction = split[0];
                int value = int.Parse(split[1]);
                distance += direction == "forward" ? value : 0;
                depth += direction switch
                {
                    "down" => value,
                    "up" => -value,
                    _ => 0
                };
            }
            
            Console.WriteLine($"Part 1. Horizontal position: {distance}, depth: {depth}, product = {distance * depth}");
        }
        
        static void Part2()
        {
            var input = File.ReadAllLines("input.txt");

            int distance = 0;
            int depth = 0;
            int aim = 0;

            foreach (string line in input)
            {
                string[] split = line.Split(' ');
                string direction = split[0];
                int value = int.Parse(split[1]);
                if (direction == "forward")
                {
                    distance += value;
                    depth += aim * value;
                }
                aim += direction switch
                {
                    "down" => value,
                    "up" => -value,
                    _ => 0
                };
            }
            
            Console.WriteLine($"Part 2. Horizontal position: {distance}, depth: {depth}, product = {distance * depth}");
        }
    }
}