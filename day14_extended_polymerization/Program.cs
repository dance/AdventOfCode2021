using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day14_extended_polymerization
{
    class Program
    {
        static void Main(string[] args)
        {
            // Part1("test_input.txt", 10);
            // SolveNaive("input.txt", 10);
            // SolveOptimized("test_input.txt", 10);
            SolveOptimized("input.txt", 10);
            SolveOptimized("input.txt", 40);
        }

        static void SolveNaive(string filename, int steps)
        {
            var rules = new Dictionary<(char, char), char>();
            var input = File.ReadAllLines(filename);
            string sourcePolymer = input[0];
            foreach (string insertionRule in input.Skip(2))
            {
                string[] split = insertionRule.Split(" -> ");
                rules.Add((split[0][0], split[0][1]), split[1][0]);
            }

            var polymer = new LinkedList<char>(sourcePolymer);
            for (int step = 0; step < steps; step++)
            {
                var modifiedPolymer = new LinkedList<char>();
                using var iter = polymer.GetEnumerator();
                iter.MoveNext();
                char c1 = iter.Current;
                modifiedPolymer.AddLast(c1);
                while (iter.MoveNext())
                {
                    char c2 = iter.Current;

                    if (rules.TryGetValue((c1, c2), out char insertion))
                        modifiedPolymer.AddLast(insertion);
                    modifiedPolymer.AddLast(c2);

                    c1 = c2;
                }

                polymer = modifiedPolymer;
                // PrintPolymer(step, polymer);
            }

            var charGroups = polymer.GroupBy(c => c).ToDictionary(c => c, chars => chars.Count());
            int leastCommon = charGroups.OrderBy(g => g.Value).First().Value;
            int mostCommon = charGroups.OrderByDescending(g => g.Value).First().Value;
            int quantity = mostCommon - leastCommon;
            Console.WriteLine($"Steps: {steps}. Quantity: {quantity}");
        }

        static void PrintPolymer(int step, LinkedList<char> polymer)
        {
            Console.Write($"After step {step+1}: ");
            foreach (char c in polymer) 
                Console.Write(c);
            Console.WriteLine();
        }

        static void SolveOptimized(string filename, int steps)
        {
            
            var rules = new Dictionary<(char, char), char>();
            var input = File.ReadAllLines(filename);
            string sourcePolymer = input[0];
            foreach (string insertionRule in input.Skip(2))
            {
                string[] split = insertionRule.Split(" -> ");
                rules.Add((split[0][0], split[0][1]), split[1][0]);
            }

            var charCounts = sourcePolymer.GroupBy(c => c).ToDictionary(chars => chars.Key,
                chars => chars.LongCount());
            
            var combinations = new Dictionary<(char, char), long>();
            for (int i = 0; i < sourcePolymer.Length - 1; i++)
            {
                char c1 = sourcePolymer[i];
                char c2 = sourcePolymer[i + 1];
                IncBy(combinations, (c1, c2), 1);
            }

            for (int step = 0; step < steps; step++)
            {
                var newCombinations = new Dictionary<(char, char), long>();
                foreach (var combination in combinations)
                {
                    long combiCount = combination.Value;
                    if (!rules.TryGetValue(combination.Key, out char insertion))
                    {
                        IncBy(newCombinations, combination.Key, combiCount);
                        continue;
                    }
                    var firstCombo = (combination.Key.Item1, insertion);
                    var secondCombo = (insertion, combination.Key.Item2);
                    IncBy(newCombinations, firstCombo, combiCount);
                    IncBy(newCombinations, secondCombo, combiCount);
                    IncBy(charCounts, insertion, combiCount);
                }

                combinations = newCombinations;
                PrintCharCounts(charCounts);
            }


            long leastCommon = charCounts.Values.Min();
            long mostCommon = charCounts.Values.Max();
            long quantity = mostCommon - leastCommon;
            Console.WriteLine($"Steps: {steps}. Quantity: {quantity}");
        }

        private static void PrintCharCounts(Dictionary<char,long> charCounts)
        {
            foreach (var charCount in charCounts)
            {
                Console.Write($"{charCount.Key}: {charCount.Value}, ");
            }
            Console.WriteLine();
        }

        static void IncBy<TKey>(Dictionary<TKey, long> dict, TKey key, long addCount)
        {
            if (!dict.ContainsKey(key))
                dict[key] = addCount;
            else
                dict[key] += addCount;
        }
    }
}