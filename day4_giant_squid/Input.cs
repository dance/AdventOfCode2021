using System.Collections.Generic;

namespace day4_giant_squid
{
    public class Input
    {
        public NumbersIndexer NumbersIndexer { get; set; }
        public IEnumerable<byte> NumbersToDraw { get; set; }
        public List<Board> Boards { get; set; }
    }
}