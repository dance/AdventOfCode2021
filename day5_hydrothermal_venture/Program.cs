using System;
using System.IO;
using System.Linq;

namespace day5_hydrothermal_venture
{
    class Program
    {
        private delegate int ProcessLineAndCountOverlaps(ref byte[,] oceanFloor, int fromX, int fromY,
            int toX, int toY);
        
        static void Main(string[] args)
        {
            Console.WriteLine("Part 1:");
            Solve(Part1_ProcessLineAndCountOverlaps);
            Console.WriteLine("Part 2:");
            Solve(Part2_ProcessLineAndCountOverlaps_CountDiagonalLines);
        }

        private static void Solve(ProcessLineAndCountOverlaps processLineAndCountOverlaps)
        {
            const int floorDimensions = 1000;
            var oceanFloor = new byte[floorDimensions, floorDimensions];
            int overlapsCount = 0;
            var input = File.ReadAllLines("input.txt");
            foreach (string line in input)
            {
                string[] splitted = line.Split(' ');
                string[] from = splitted[0].Split(',');
                string[] to = splitted[2].Split(',');
                int fromX = int.Parse(from[0]);
                int fromY = int.Parse(from[1]);
                int toX = int.Parse(to[0]);
                int toY = int.Parse(to[1]);
                overlapsCount += processLineAndCountOverlaps(ref oceanFloor, fromX, fromY, toX, toY);
            }
            // Console.WriteLine("Ocean floor looks like:");
            // for (int i = 0; i < floorDimensions; i++)
            // {
            //     for (int j = 0; j < floorDimensions; j++)
            //     {
            //         Console.Write(oceanFloor[i, j] == 0 ? "." : oceanFloor[i, j].ToString());
            //     }
            //     Console.WriteLine();
            // }
            Console.WriteLine($"Points count where at least two lines overlap is {overlapsCount}");
        }

        private static int Part1_ProcessLineAndCountOverlaps(ref byte[,] oceanFloor, int fromX, int fromY,
            int toX, int toY)
        {
            if (fromX != toX && fromY != toY) // skip diagonal lines
                return 0; 
            
            int overlapsCount = 0;
            foreach (int i in (fromX, toX).EnumerateRange())
            foreach (int j in (fromY, toY).EnumerateRange())
                overlapsCount += MarkFloorCell(ref oceanFloor, i, j);

            return overlapsCount;
        }
        
        private static int Part2_ProcessLineAndCountOverlaps_CountDiagonalLines(ref byte[,] oceanFloor, int fromX, int fromY,
            int toX, int toY)
        {
            int overlapsCount = 0;
            if (fromX == toX || fromY == toY)
            {
                foreach (int i in (fromX, toX).EnumerateRange())
                foreach (int j in (fromY, toY).EnumerateRange())
                    overlapsCount += MarkFloorCell(ref oceanFloor, i, j);
            }
            else
            {
                foreach ((int i, int j) in (fromX, toX).EnumerateRange().Zip((fromY, toY).EnumerateRange()))
                    overlapsCount += MarkFloorCell(ref oceanFloor, i, j);
            }

            return overlapsCount;
        }

        private static int MarkFloorCell(ref byte[,] oceanFloor, int i, int j)
        {
            int overlapsCount = 0;
            oceanFloor[i, j]++;
            if (oceanFloor[i, j] == 2)
                overlapsCount++;
            return overlapsCount;
        }
    }
}