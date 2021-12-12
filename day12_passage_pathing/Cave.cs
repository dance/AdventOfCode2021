using System.Collections.Generic;
using System.Linq;

namespace day12_passage_pathing
{
    public record Cave(string Name)
    {
        public HashSet<Cave> Connections { get; } = new();
        
        public bool IsLarge() => Name.Any(c => c < 'a');

        public override string ToString()
        {
            return $"{Name} -> {string.Join(", ", Connections.Select(c => c.Name))}";
        }
    }
}