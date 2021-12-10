using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace day10_syntax_scoring
{
    class Program
    {
        static void Main(string[] args)
        {
            var completions = SolvePart1AndGetCompletionsForIncompleteLines();
            SolvePart2(completions);
        }

        static Dictionary<char, int> CloseBracketsScore = new()
        {
            {')', 3},
            {']', 57},
            {'}', 1197},
            {'>', 25137},
        };

        private static Dictionary<char, char> BracketPairs = new()
        {
            {'(', ')'},
            {'[', ']'},
            {'{', '}'},
            {'<', '>'},
        };

        static List<string> SolvePart1AndGetCompletionsForIncompleteLines()
        {
            string[] input = File.ReadAllLines("input.txt");
            List<string> completions = new List<string>(input.Length);
            int syntaxErrorsScore = 0;
            foreach (string line in input)
            {
                int errorScore = 0;
                var stack = new Stack<char>();
                foreach (char currentBracket in line)
                {
                    if (BracketPairs.ContainsKey(currentBracket))
                    {
                        stack.Push(currentBracket);
                        continue;
                    }

                    char prevBracket = stack.Pop();
                    if (!BracketPairs.Contains(new KeyValuePair<char, char>(prevBracket, currentBracket)))
                    {
                        // we got syntax error - corrupted line
                        errorScore += CloseBracketsScore[currentBracket];
                        break;
                    }
                }

                if (stack.Count > 0 && errorScore == 0)
                {
                    // not corrupted and has non-matched brackets? then it's incomplete, add completions list
                    completions.Add(stack.Aggregate("", (agg, c) => agg + BracketPairs[c]));
                }
                
                // if line is corrupted, add score
                syntaxErrorsScore += errorScore;
            }
            
            Console.WriteLine($"Part 1. Syntax errors score is {syntaxErrorsScore}");
            return completions;
        }

        static void SolvePart2(List<string> completions)
        {
            Debug.Assert(completions.Count % 2 == 1);
            
            var bracketScores = new Dictionary<char, uint>
            {
                {')', 1},
                {']', 2},
                {'}', 3},
                {'>', 4}
            };
            ulong[] scores = new ulong[completions.Count];
            for (int i = 0; i < completions.Count; i++)
            {
                string completion = completions[i];
                ulong score = 0;
                foreach (char bracket in completion)
                    score = score * 5 + bracketScores[bracket];
                scores[i] = score;
            }

            Array.Sort(scores);
            Console.WriteLine($"Scores: {string.Join(", ", scores)}. Middle score: {scores[scores.Length / 2]}");
        }
    }
}