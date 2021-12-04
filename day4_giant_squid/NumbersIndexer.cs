using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace day4_giant_squid
{
    // Number -> List of indexes; each index is combined of board index, and position in that board
    // (2 bytes index:
    //    - 1st byte is board index (test input had 100 of them, so...),
    //    - 2nd byte contains index of number in a 5x5 board in form i*5+j (like flat array index) to
    //      pack it in 5 bits (since board is 5x5 indexes are in range 0..25 that fits into 2^5)
    // i.e. if number 13 in board 2 has index [4, 4] then its combined index is
    // numberIndex = 2 << 8 + (4 * 5 + 4) = 10_0001_1000b = 536
    public class NumbersIndexer
    {
        private Dictionary<byte, List<ushort>> NumbersIndexes { get; } = new();

        public void Add(byte number, byte boardIndex, byte i, byte j)
        {
            List<ushort> indexes;
            if (!NumbersIndexes.TryGetValue(number, out indexes))
                NumbersIndexes[number] = indexes = new List<ushort>();
            indexes.Add(CombineIndex(boardIndex, i, j));
        }

        private ushort CombineIndex(byte boardIndex, byte i, byte j)
        {
            return (ushort)((boardIndex << 8) + (i * (ushort)Board.BOARD_SIZE + j));
        }

        private (byte boardIdx, byte i, byte j) DecomposeIndex(ushort combinedIdx)
        {
            byte boardIdx = (byte)(combinedIdx >> 8);
            byte indexes = (byte)(combinedIdx & Board.UNMASK);
            byte i = (byte) (indexes / Board.BOARD_SIZE);
            byte j = (byte) (indexes % Board.BOARD_SIZE);
            return (boardIdx, i, j);
        }

        public IEnumerable<(byte boardIdx, byte i, byte j)> GetPositions(byte number)
        {
            return NumbersIndexes[number].Select(DecomposeIndex);
        }
    }
}