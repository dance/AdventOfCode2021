using System;
using System.IO;

namespace day3_binary_diagnostic
{
    class Program
    {
        private const int BITS_COUNT = 12;
            
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var tree = new Node();
            foreach (string line in input)
            {
                var treeNode = tree;
                foreach (char c in line)
                {
                    switch (c)
                    {
                        case '0':
                            treeNode.Zeroes++;
                            treeNode.Zero ??= new Node();
                            treeNode = treeNode.Zero;
                            break;
                        case '1':
                            treeNode.Ones++;
                            treeNode.One ??= new Node();
                            treeNode = treeNode.One;
                            break;
                        default: throw new ArgumentException();
                    }
                }
            }

            var node = tree;
            int oxygenGeneratorRating = 0;
            for (int i = BITS_COUNT - 1; i >= 0; i--)
            {
                if (node.Ones + node.Zeroes == 1)
                {
                    if (node.One != null)
                        oxygenGeneratorRating += 1 << i;
                    node = node.One ?? node.Zero;
                }
                else if (node.Ones >= node.Zeroes)
                {
                    oxygenGeneratorRating += 1 << i;
                    node = node.One;
                }
                else
                {
                    node = node.Zero;
                }
            }
            
            node = tree;
            int co2ScrubberRating = 0;
            for (int i = BITS_COUNT - 1; i >= 0; i--)
            {
                if (node.Ones + node.Zeroes == 1)
                {
                    if (node.One != null)
                        co2ScrubberRating += 1 << i;
                    node = node.One ?? node.Zero;
                }
                else if (node.Ones < node.Zeroes)
                {
                    co2ScrubberRating += 1 << i;
                    node = node.One;
                }
                else
                {
                    node = node.Zero;
                }
            }
            
            Console.WriteLine($"Part 2. Life support rating: {oxygenGeneratorRating * co2ScrubberRating}");
        }
    }
}