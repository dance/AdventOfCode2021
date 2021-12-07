using System;

namespace day7_treachery_of_whales
{
    // https://en.wikipedia.org/wiki/Quickselect
    public static class Quickselect
    {
        private static Random _random = new Random(1366613);
        
        public static int QuickselectMedian(int[] input)
        {
            return DoQuickselect(input, 0, input.Length - 1, input.Length / 2);
        }

        // Returns the k-th smallest element of list within left..right inclusive
        // (i.e. left <= k <= right).
        private static int DoQuickselect(int[] input, int leftIdx, int rightIdx, int k)
        {
            if (leftIdx == rightIdx)
                return input[leftIdx];
            int pivotIndex = _random.Next(leftIdx + 1, rightIdx);
            pivotIndex = Partition(input, leftIdx, rightIdx, pivotIndex);
            if (k == pivotIndex)
                return input[k];
            else if (k < pivotIndex)
                return DoQuickselect(input, leftIdx, pivotIndex - 1, k);
            else
                return DoQuickselect(input, pivotIndex + 1, rightIdx, k);
        }

        private static int Partition(int[] input, int leftIdx, int rightIdx, int pivotIdx)
        {
            int pivot = input[pivotIdx];
            Swap(pivotIdx, rightIdx);
            int storeIndex = leftIdx;
            for (int i = leftIdx; i < rightIdx - 1; i++)
                if (input[i] < pivot)
                    Swap(storeIndex++, i);

            void Swap(int firstIdx, int secondIdx)
            {
                // swap via deconstruction
                (input[firstIdx], input[secondIdx]) = (input[secondIdx], input[firstIdx]);
            }

            return storeIndex;
        }
    }
}