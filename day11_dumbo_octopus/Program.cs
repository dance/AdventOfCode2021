using System;
using System.IO;
using static System.Math;
using static day11_dumbo_octopus.Consts;

namespace day11_dumbo_octopus
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
            int[,] octopuses = ReadInput();

            int totalFlashes = 0;
            for (int step = 0; step < Steps; step++)
            {
                IncreaseEnergy(ref octopuses);
                int flashesThisStep = ProcessFlashes(ref octopuses);
                ResetFlashedOctopuses(ref octopuses);
                
                // Console.WriteLine($"Part 1. Flashes during step {step + 1}: {flashesThisStep}");
                totalFlashes += flashesThisStep;
                // PrintOctopuses(ref octopuses);
            }
            
            Console.WriteLine($"Part 1. Total flashes after {Steps} steps: {totalFlashes}");
        }

        static int[,] ReadInput()
        {
            var octopuses = new int[DIM, DIM];
            // var input = File.ReadAllLines("test_input.txt");
            var input = File.ReadAllLines("input.txt");
            for (int i = 0; i < DIM; i++)
            for (int j = 0; j < DIM; j++)
                octopuses[i, j] = input[i][j] - '0';
            return octopuses;
        }

        static void IncreaseEnergy(ref int[,] octopuses)
        {
            // First, the energy level of each octopus increases by 1.
            for (int i = 0; i < DIM; i++)
            for (int j = 0; j < DIM; j++)
                octopuses[i, j]++;
        }

        static int ProcessFlashes(ref int[,] octopuses)
        {
            /*
             * Then, any octopus with an energy level greater than 9 flashes.
             * This increases the energy level of all adjacent octopuses by 1, including octopuses
             * that are diagonally adjacent. If this causes an octopus to have an energy level greater than 9,
             * it also flashes. This process continues as long as new octopuses keep having their energy level
             * increased beyond 9. (An octopus can only flash at most once per step.)
             */
            int flashesThisStep = 0;
            for (int i = 0; i < DIM; i++)
            for (int j = 0; j < DIM; j++)
            {
                if (octopuses[i, j].Flashed())
                    continue;
                if (octopuses[i, j] <= EnergyLevelToFlash)
                    continue;
                flashesThisStep += Flash(ref octopuses, i, j);
            }
            return flashesThisStep;
        }

        static int Flash(ref int[,] octopuses, int i, int j)
        {
            if (octopuses[i, j].Flashed())
                return 0;
            octopuses[i, j] |= FLASHED_BIT;
            int flashes = 1;
            
            for (int x = Max(0, i - 1); x <= Min(DIM - 1, i + 1); x++)
            for (int y = Max(0, j - 1); y <= Min(DIM - 1, j + 1); y++)
            {
                if (x == i && y == j) continue;
                if (++octopuses[x, y] > EnergyLevelToFlash)
                    flashes += Flash(ref octopuses, x, y);
            }

            return flashes;
        }

        static void ResetFlashedOctopuses(ref int[,] octopuses)
        {
            // Finally, any octopus that flashed during this step has its energy level set to 0,
            // as it used all of its energy to flash.
            for (int i = 0; i < DIM; i++)
            for (int j = 0; j < DIM; j++)
                octopuses[i, j] = octopuses[i, j].Flashed() ? 0 : octopuses[i, j];
        }
        
        static void PrintOctopuses(ref int[,] octopuses)
        {
            for (int i = 0; i < DIM; i++)
            {
                for (int j = 0; j < DIM; j++)
                    Console.Write(octopuses[i, j]);
                Console.WriteLine();
            }
        }
        
        static void Part2()
        {
            int[,] octopuses = ReadInput();

            int step = 0;
            while (true)
            {
                step++;
                IncreaseEnergy(ref octopuses);
                int flashesThisStep = ProcessFlashes(ref octopuses);
                ResetFlashedOctopuses(ref octopuses);
                
                if (flashesThisStep == DIM * DIM)
                    break;
            }
            
            Console.WriteLine($"Part 2. All octopuses flashed simultaneously on step {step}");
        }
                
    }
}