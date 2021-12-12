using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace day12_passage_pathing
{
    class Program
    {
        static void Main(string[] args)
        {
            // Part1("test_input.txt");
            // Part1("test_input2.txt");
            // Part1("test_input3.txt");
            Part1("input.txt", printPaths: false);
            
            // Part2("test_input.txt", printPaths: false);
            // Part2("test_input2.txt", printPaths: false);
            // Part2("test_input3.txt", printPaths: false);
            Part2("input.txt", printPaths: false);
        }

        static void Part1(string filename, bool printPaths)
        {
            Console.WriteLine($"Input: {filename}");
            var caveSystem = ReadInput(filename);

            var paths = new PathFinder(caveSystem).ToList();
            if (printPaths)
                foreach (var path in paths)
                {
                    Console.WriteLine(string.Join(',', path.Select(c => c.Name)));
                }
            Console.WriteLine($"Part 1. Total paths: {paths.Count}");
        }

        static void Part2(string filename, bool printPaths)
        {
            Console.WriteLine($"Input: {filename}");
            var caveSystem = ReadInput(filename);

            var totalPaths = new List<string>();
            var smallCaves = caveSystem.Caves
                .Where(c => !c.IsLarge() && c.Name != "start" && c.Name != "end");
            foreach (var smallCaveCanVisitTwice in smallCaves)
            {
                var paths = new PathFinderCanVisitTwice(caveSystem, smallCaveCanVisitTwice).ToList();
                totalPaths.AddRange(paths.Select(path => string.Join(',', path.Select(c => c.Name))));
            }
            if (printPaths)
                foreach (var path in totalPaths.Distinct())
                    Console.WriteLine(path);
            Console.WriteLine($"Part 2. Total paths: {totalPaths.Distinct().Count()}");
        }

        static CaveSystem ReadInput(string filename)
        {
            var input = File.ReadAllLines(filename, Encoding.ASCII).ToList();
            var caveSystem = new CaveSystem();
            foreach (string path in input)
            {
                string[] caves = path.Split('-');
                caveSystem.AddConnection(caves[0], caves[1]);
            }
            return caveSystem;
        }
    }
}