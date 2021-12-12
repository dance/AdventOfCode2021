using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace day12_passage_pathing
{
    public class PathFinder : IEnumerable<List<Cave>>
    {
        private CaveSystem CaveSystem { get; }
        
        public PathFinder(CaveSystem caveSystem)
        {
            CaveSystem = caveSystem;
        }


        // returns paths from start to end
        public IEnumerator<List<Cave>> GetEnumerator()
        {
            var startCave = CaveSystem.Caves.Single(c => c.Name == "start");
            var visitedCaves = new HashSet<Cave>();
            var paths = GetPathsToEnd(startCave, visitedCaves);
            foreach (var path in paths)
                yield return path;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private List<List<Cave>> GetPathsToEnd(Cave fromCave, HashSet<Cave> visitedCaves)
        {
            var pathFromCurrentCave = new List<Cave> {fromCave};
            var result = new List<List<Cave>>();
            
            if (fromCave.Name == "end")
                return new List<List<Cave>> {pathFromCurrentCave};
            
            if (!fromCave.IsLarge())
            {
                if (visitedCaves.Contains(fromCave))
                    return result;
                
                visitedCaves.Add(fromCave);
            }

            foreach (var connectedCave in fromCave.Connections)
            {
                var pathsToEnd = GetPathsToEnd(connectedCave, visitedCaves.ToHashSet());
                foreach (var pathToEnd in pathsToEnd) 
                    result.Add(pathFromCurrentCave.Concat(pathToEnd).ToList());
            }
            
            return result;
        }
    }
}