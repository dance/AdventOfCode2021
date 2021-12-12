using System.Collections.Generic;
using System.Linq;

namespace day12_passage_pathing
{
    public class CaveSystem
    {
        public List<Cave> Caves { get; } = new();

        public void AddConnection(string fromCaveName, string toCaveName)
        {
            var fromCave = GetOrAddCave(fromCaveName);
            var toCave = GetOrAddCave(toCaveName);
            AddConnection(fromCave, toCave);
        }

        private void AddConnection(Cave fromCave, Cave toCave)
        {
            fromCave.Connections.Add(toCave);
            toCave.Connections.Add(fromCave);
        }

        private Cave GetOrAddCave(string name)
        {
            var cave = Caves.FirstOrDefault(c => c.Name == name);
            if (cave == null)
            {
                cave = new Cave(name);
                Caves.Add(cave);
            }
            return cave;
        }
    }
}