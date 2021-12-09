using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace day9_smoke_basin
{
    class Program
    {
        static void Main(string[] args)
        {
            Part1();
            Part2();
        }

        const int COUNT = 100;

        static int[,] ReadInput()
        {
            int[,] input = new int[COUNT, COUNT];
            var inputLines = File.ReadAllLines("input.txt");
            for (int i = 0; i < COUNT; i++)
            for (int j = 0; j < COUNT; j++)
                input[i, j] = inputLines[i][j] - '0';
            return input;
        }

        static IEnumerable<(int i, int j)> GetLowestPoints(int[,] input)
        {
            for (int i = 0; i < COUNT; i++)
            for (int j = 0; j < COUNT; j++)
            {
                if ((i - 1 < 0 || input[i - 1, j] > input[i, j]) &&
                    (i + 1 >= COUNT || input[i, j] < input[i + 1, j]) &&
                    (j - 1 < 0 || input[i, j - 1] > input[i, j]) &&
                    (j + 1 >= COUNT || input[i, j] < input[i, j + 1]))
                {
                    yield return (i, j);
                }
            }
        }

        static void Part1()
        {
            int[,] input = ReadInput();

            int riskLevel = 0; 
            foreach ((int i, int j) in GetLowestPoints(input))
                riskLevel += 1 + input[i, j];
            
            Console.WriteLine($"Part 1. Risk level = {riskLevel}");
        }

        static void Part2()
        {
            int[,] input = ReadInput();

            var lowestPoints = GetLowestPoints(input);
            var basinSizes = new List<int>(100);
            foreach ((int x, int y) in lowestPoints)
            {
                int basinSize = GetBasinSize(ref input, Direction.Left, x, y - 1) +
                                GetBasinSize(ref input, Direction.Up, x - 1, y) +
                                GetBasinSize(ref input, Direction.Right, x, y + 1) +
                                GetBasinSize(ref input, Direction.Down, x + 1, y);
                basinSizes.Add(basinSize);
            }

            int basinSizesMultiplied = basinSizes.OrderByDescending(s => s).Take(3).Aggregate(1, (s, i) => s *= i);
            Console.WriteLine($"Part 2. Sizes of the three largest basins multiplied: {basinSizesMultiplied}");
        }

        enum Direction
        {
            Left,
            Up,
            Right,
            Down
        }

        static int GetBasinSize(ref int[,] input, Direction direction, int i, int j)
        {
            if (i < 0 || j < 0 || i >= COUNT || j >= COUNT)
                return 0;
            if (input[i, j] == 9)
                return 0;
            input[i, j] = 9; // mark as visited
            int size = 1;
            size += direction != Direction.Down ? GetBasinSize(ref input, Direction.Up, i - 1, j) : 0;
            size += direction != Direction.Left ? GetBasinSize(ref input, Direction.Right, i, j + 1) : 0;
            size += direction != Direction.Right ? GetBasinSize(ref input, Direction.Left, i, j - 1) : 0;
            size += direction != Direction.Up ? GetBasinSize(ref input, Direction.Down, i + 1, j) : 0;
            return size;
        }
    }
}