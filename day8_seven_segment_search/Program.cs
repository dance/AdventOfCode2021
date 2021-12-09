using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace day8_seven_segment_search
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
            var input = File.ReadAllLines("input.txt");
            int count1478 = 0;
            foreach (string line in input)
            {
                var split = line.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                string[] digitsInput = split[0].Split(' ');
                string[] digitsOutput = split[1].Split(' ');
                foreach (string digitOut in digitsOutput)
                {
                    int digitLen = digitOut.Length;
                    if (new[] {2, 3, 4, 7}.Contains(digitLen))
                        count1478++;
                }
            }
            Console.WriteLine($"Part 1. Count of 1, 4, 7, 8: {count1478}");
        }
        
        const string Zero = "abcefg";
        const string One = "cf";
        const string Two = "acdeg";
        const string Three = "acdfg";
        const string Four = "bcdf";
        const string Five = "abdfg";
        const string Six = "abdefg";
        const string Seven = "acf";
        const string Eight = "abcdefg";
        const string Nine = "abcdfg";

        static Dictionary<int, string> Digits = new()
        {
            {0, Zero},
            {1, One},
            {2, Two},
            {3, Three},
            {4, Four},
            {5, Five},
            {6, Six},
            {7, Seven},
            {8, Eight},
            {9, Nine}
        };
        
        static Dictionary<char, int> _indexes = new()
        {
            {'a', 0},
            {'c', 1},
            {'f', 2},
            {'b', 3},
            {'d', 4},
            {'e', 5},
            {'g', 6}
        };

        static void Part2()
        {
            var input = File.ReadAllLines("input.txt").Select(l =>
            {
                var split = l.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                return new {digitsInput = split[0].Split(' '), digitsOutput = split[1].Split(' ')};
            });
            int sum = 0;
            foreach (var entry in input)
            {
                string[] inDigitsSorted = entry.digitsInput.OrderBy(d => d.Length).ToArray();
                Debug.Assert(inDigitsSorted.Length == 10, "expected all digits from 0 to 9");
                char[] one = inDigitsSorted[0].ToCharArray();
                char[] seven = inDigitsSorted[1].ToCharArray();
                char[] four = inDigitsSorted[2].ToCharArray();
                Debug.Assert(one.Length == 2, "expected to get 'one'");
                Debug.Assert(seven.Length == 3, "expected to get 'seven'");
                Debug.Assert(four.Length == 4, "expected to get 'four'");
                // after analyzing these three digits we have following:
                // one segment 'a' mapped exactly (a in 7, as 1=cf and 7=acf)
                // two segments ('c','f') paired mapped to another pair
                // two segments ('b','d') paired mapped to another pair (as 4 = bcdf and cf pair is mapped)
                // last two segments ('e','g') paired mapped to last possible pair
                char[] a = seven.Except(one).ToArray(); // single element in fact; char[] for convenience
                char[] cf = one;
                char[] bd = four.Except(cf).ToArray();
                char[] eg = Eight.Except(a).Except(cf).Except(bd).ToArray();
                Debug.Assert(a.Length == 1, "a.Length == 1");
                Debug.Assert(cf.Length == 2, "cf.Length == 2");
                Debug.Assert(bd.Length == 2, "bd.Length == 2");
                Debug.Assert(eg.Length == 2, "eg.Length == 2");
                // now we make all possible remap combinations and check them against input
                string remap = FindRemap(inDigitsSorted, a, cf, bd, eg);
                // decode using remap
                int decodedNumber = 0;
                for (int i = 0; i < entry.digitsOutput.Length; i++)
                {
                    string digitOut = entry.digitsOutput[i];
                    decodedNumber += DecodeDigit(digitOut, remap) * (int)Math.Pow(10, entry.digitsOutput.Length - i - 1);
                }
                sum += decodedNumber;
            }
            Console.WriteLine($"Part 2. Sum of all decoded values is {sum}");

            string FindRemap(string[] inDigitsSorted, char[] a, char[] cf, char[] bd, char[] eg)
            {
                foreach (char[] cfp in cf.Permute())
                foreach (char[] bdp in bd.Permute())
                foreach (char[] egp in eg.Permute())
                {
                    string remap = string.Concat(a.Union(cfp).Union(bdp).Union(egp));
                    bool match = true;
                    foreach (string digit in Digits.Values)
                    {
                        string digitRemapped = string.Concat(digit.Select(d => remap[_indexes[d]]));
                        match &= inDigitsSorted.Any(inDigit =>
                            inDigit.OrderBy(d => d).SequenceEqual(digitRemapped.OrderBy(d => d)));
                    }
                    if (match)
                        return remap;
                }
                throw new Exception("Remap not found");
            }

            int DecodeDigit(string digit, string remap)
            {
                // remap is like a + cf + bd + eg
                char[] decodedDigit = digit.Select(d => _indexes.Single(i => i.Value == remap.IndexOf(d)).Key)
                    .OrderBy(d => d).ToArray();
                return Digits.Single(d => d.Value.SequenceEqual(decodedDigit)).Key;
            }
        }
    }
}