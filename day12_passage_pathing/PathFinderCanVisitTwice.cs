using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace day12_passage_pathing
{
    public class PathFinderCanVisitTwice : IEnumerable<List<Cave>>
    {
        private CaveSystem CaveSystem { get; }
        private string CaveCanVisitTwiceName { get; }
        
        public PathFinderCanVisitTwice(CaveSystem caveSystem, Cave caveCanVisitTwice)
        {
            CaveSystem = caveSystem;
            CaveCanVisitTwiceName = caveCanVisitTwice.Name;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<List<Cave>> GetEnumerator()
        {
            var startCave = CaveSystem.Caves.Single(c => c.Name == "start");
            var visitedCaves = new Dictionary<Cave, int>();
            var paths = GetPathsToEnd(startCave, visitedCaves);
            foreach (var path in paths)
                yield return path;
        }
        
        private List<List<Cave>> GetPathsToEnd(Cave fromCave, Dictionary<Cave, int> visitedCaves)
        {
            var pathFromCurrentCave = new List<Cave> {fromCave};
            var result = new List<List<Cave>>();
            
            if (fromCave.Name == "end")
                return new List<List<Cave>> {pathFromCurrentCave};
            
            if (!fromCave.IsLarge())
            {
                if (visitedCaves.TryGetValue(fromCave, out int visits))
                {
                    // cannot visit regular small cave twice
                    if (fromCave.Name != CaveCanVisitTwiceName)
                        return result;
                    // cannot visit special cave more than twice
                    if (visits >= 2)
                        return result;
                    visitedCaves[fromCave] = 2;
                }
                else
                    visitedCaves.Add(fromCave, 1);
            }

            foreach (var connectedCave in fromCave.Connections)
            {
                var pathsToEnd = GetPathsToEnd(connectedCave, new Dictionary<Cave, int>(visitedCaves));
                foreach (var pathToEnd in pathsToEnd) 
                    result.Add(pathFromCurrentCave.Concat(pathToEnd).ToList());
            }
            
            return result;
        }
    }
}