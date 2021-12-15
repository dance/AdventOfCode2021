using System;
using System.Diagnostics;
using System.IO;

namespace day15_chiton;

class Program
{
    static void Main(string[] args)
    {
        // Part1("test_input.txt", 10);
        Part1("input.txt", 100);
        // Part2("test_input.txt", 10);
        Part2("input.txt", 100);
    }

    static void Part1(string filename, int dim)
    {
        int[,] map = new int[dim, dim];
        var input = File.ReadAllLines(filename);
            
        for (int i = 0; i < dim; i++)
        for (int j = 0; j < dim; j++)
            map[i, j] = input[i][j] - '0';
        // PrintMap(map, dim);

        var pathFinder = new ShortestPathFinder(map, dim);
        Console.WriteLine($"Part 1. Total shortest (minimal risk) path cost: {pathFinder.Solution.Cost.TotalCost}");
    }

    private static void PrintMap(int[,] map, int dim)
    {
        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
                Console.Write(map[i, j]);
            Console.WriteLine();
        }
    }

    static void Part2(string filename, int dim)
    {
        int[,] map = new int[dim, dim];
        var input = File.ReadAllLines(filename);
            
        for (int i = 0; i < dim; i++)
        for (int j = 0; j < dim; j++)
            map[i, j] = input[i][j] - '0';

        int newDim = dim * 5;
        var fullMap = new int[newDim, newDim];
        for (int i = 0; i < newDim; i++)
        {
            int sectionX = i / dim;
            for (int j = 0; j < newDim; j++)
            {
                int newValue = map[i % dim, j % dim] + sectionX + j / dim;
                if (newValue >= 10)
                    newValue -= 9;
                Debug.Assert(newValue >= 1 && newValue <= 9);
                fullMap[i, j] = newValue;
            }
        }
        // PrintMap(fullMap, newDim);
        var pathFinder = new ShortestPathFinder(fullMap, newDim);
        Console.WriteLine($"Part 2. Total shortest (minimal risk) path cost: {pathFinder.Solution.Cost.TotalCost}");
    }
}