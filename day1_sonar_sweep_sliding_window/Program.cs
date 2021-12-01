using System;
using System.IO;
using System.Linq;

namespace day1_sonar_sweep_sliding_window
{
    class Program
    {
        private const int WINDOW_SIZE = 3;
        
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt").Select(int.Parse).ToArray();

            var oldWindow = new int[WINDOW_SIZE] { input[0], input[1], input[2] };
            var currentWindow = new int[WINDOW_SIZE] { input[1], input[2], input[3] };

            int windowIncreasedCount = 0;

            for (int i = 4; i <= input.Length; i++)
            {
                int oldSum = oldWindow.Sum();
                int currentSum = currentWindow.Sum();
                if (currentSum > oldSum)
                    windowIncreasedCount++;
                
                ShiftWindow(oldWindow, currentWindow[2]);
                if (i == input.Length)
                    break;
                ShiftWindow(currentWindow, input[i]);
            }
            
            Console.WriteLine($"Window increased count: {windowIncreasedCount}");
        }

        static void ShiftWindow(int[] window, int newDepth)
        {
            for (int i = 0; i < WINDOW_SIZE - 1; i++)
            {
                window[i] = window[i + 1];
            }
            window[WINDOW_SIZE - 1] = newDepth;
        }
    }
}