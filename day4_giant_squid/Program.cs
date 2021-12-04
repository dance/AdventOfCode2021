using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace day4_giant_squid
{
    class Program
    {
        static void Main(string[] args)
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            var input = ReadInput();
            foreach (byte number in input.NumbersToDraw)
            {
                foreach ((byte boardIdx, byte i, byte j) in input.NumbersIndexer.GetPositions(number))
                {
                    bool boardWins = input.Boards[boardIdx].Mark(i, j, out int score);
                    if (boardWins)
                    {
                        Console.WriteLine($"Part 1. Board {boardIdx} wins with score {score}");
                        return;
                    }
                }
            }
        }

        private static void Part2()
        {
            var input = ReadInput();
            var boardsWon = new HashSet<int>();
            int totalBoards = input.Boards.Count;
            foreach (byte number in input.NumbersToDraw)
            {
                foreach ((byte boardIdx, byte i, byte j) in input.NumbersIndexer.GetPositions(number))
                {
                    bool boardWins = input.Boards[boardIdx].Mark(i, j, out int score);
                    if (boardWins)
                        boardsWon.Add(boardIdx);
                    if (boardsWon.Count == totalBoards)
                    {
                        Console.WriteLine($"Part 2. Score of last board {boardIdx} that won is {score}");
                        return;
                    }
                }
            }

        }

        private static Input ReadInput()
        {
            var result = new Input();
            
            var input = File.ReadAllLines("input.txt");
            result.NumbersToDraw = input[0].Split(',').Select(byte.Parse);

            result.NumbersIndexer = new NumbersIndexer();
            result.Boards = new List<Board>(100);
            byte boardCount = 0;
            int currentInputStr = 2; // skip first line and second empty line
            while (currentInputStr < input.Length)
            {
                var board = new Board();
                for (byte i = 0; i < Board.BOARD_SIZE; i++)
                {
                    byte[] numbersLine = input[currentInputStr + i]
                        .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                        .Select(byte.Parse)
                        .ToArray();
                    Debug.Assert(numbersLine.Length == Board.BOARD_SIZE);
                    for (byte j = 0; j < Board.BOARD_SIZE; j++)
                    {
                        board.Numbers[i, j] = numbersLine[j];
                        result.NumbersIndexer.Add(numbersLine[j], boardCount, i, j);
                    }
                }

                result.Boards.Add(board);
                boardCount++;
                currentInputStr += Board.BOARD_SIZE + 1; // skip empty line after board too
            }

            return result;
        }
    }
}