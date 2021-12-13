using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day13_transparent_origami
{
    class Program
    {
        static void Main(string[] args)
        {
            // Part1("test_input.txt", 11, 15, 2, true);
            Solve("input.txt", 1311, 895, 1, false);
            Solve("input.txt", 1311, 895, 20, true);
        }

        static void Solve(string filename, int dimX, int dimY, int foldsToDo, bool printResult)
        {
            int[,] paper = new int[dimY, dimX];
            List<string> instructions = new List<string>();
            foreach (var line in File.ReadAllLines(filename))
            {
                if (string.IsNullOrEmpty(line))
                    continue;
                if (line.StartsWith("fold"))
                {
                    instructions.Add(line);
                    continue;
                }

                string[] split = line.Split(',');
                int x = int.Parse(split[0]);
                int y = int.Parse(split[1]);
                paper[y, x] = 1;
            }
            // PrintPaper(ref paper, dimX, dimY);

            for (int foldId = 0; foldId < instructions.Count && foldId < foldsToDo; foldId++)
            {
                string fold = instructions[foldId];
                char axis = fold.Skip(11).Take(1).Single();
                int foldIndex = int.Parse(new string(fold.Skip(13).ToArray()));
                Console.WriteLine($"Folding by '{axis}', line {foldIndex}");
                if (axis == 'y')
                {
                    for (int y = foldIndex + 1; y < dimY; y++)
                    for (int x = 0; x < dimX; x++)
                        paper[foldIndex - (y - foldIndex), x] |= paper[y, x];

                    dimY = foldIndex;
                }
                else
                {
                    for (int y = 0; y < dimY; y++)
                    for (int x = foldIndex + 1; x < dimX; x++)
                        paper[y, foldIndex - (x - foldIndex)] |= paper[y, x];

                    dimX = foldIndex;
                }
                // PrintPaper(ref paper, dimX, dimY);
            }

            int visibleDots = 0;
            for (int y = 0; y < dimY; y++)
            for (int x = 0; x < dimX; x++)
                visibleDots += paper[y, x];
            Console.WriteLine($"Dots visible after {Math.Min(foldsToDo, instructions.Count)} folds: {visibleDots}");
            
            if (printResult)
                PrintPaper(ref paper, dimX, dimY);
        }

        static void PrintPaper(ref int[,] paper, int dimX, int dimY)
        {
            for (int y = 0; y < dimY; y++)
            {
                for (int x = 0; x < dimX; x++)
                {
                    Console.Write(paper[y, x] == 0 ? '.' : '∎');
                }

                Console.WriteLine();
            }
        }
}
}