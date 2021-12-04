using System;
using System.Linq;

namespace day4_giant_squid
{
    public class Board
    {
        public const int BOARD_SIZE = 5;
        private const ushort MASK = 1 << 8;
        public const ushort UNMASK = unchecked((ushort)~MASK);

        // numbers are 0..99, so 1 byte to store number itself, and another byte to mark them
        public ushort[,] Numbers { get; set; } = new ushort[BOARD_SIZE, BOARD_SIZE];

        // sum of all unchecked numbers times last called number
        public int GetScore(int lastCalledNumber)
        {
            int sum = Numbers.Cast<ushort>().Where(number => number < MASK).Select(n => (int)n).Sum();
            return sum * lastCalledNumber;
        }

        // returns true if board wins - has a row or column of marked numbers
        public bool Mark(byte i, byte j, out int score)
        {
            Numbers[i, j] |= MASK;
            if (IsRowMarked(i) || IsColumnMarked(j))
            {
                score = GetScore(Numbers[i, j] & UNMASK);
                return true;
            }
            score = 0;
            return false;
        }

        private bool IsRowMarked(byte i) => Enumerable.Range(0, BOARD_SIZE).All(j => Numbers[i, j] >= MASK);
        private bool IsColumnMarked(byte j) => Enumerable.Range(0, BOARD_SIZE).All(i => Numbers[i, j] >= MASK);
    }
}