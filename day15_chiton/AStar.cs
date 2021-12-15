using System;
using System.Collections.Generic;

namespace day15_chiton;

public abstract class AStar
{
    protected void FindPathFrom(Node start, PriorityQueue<Node, int> openList, Dictionary<Point, Cost> closedList)
    {
        openList.Enqueue(start, 0);
        while (openList.Count > 0)
        {
            Node node = openList.Dequeue();
            if (closedList.ContainsKey(node.Position))
                continue;
            closedList.Add(node.Position, node.Cost);
            if (IsDestination(node)) 
                return;
            AddNeighbours(node, openList);
        }
    }

    protected abstract void AddNeighbours(Node node, PriorityQueue<Node, int> openList);
    protected abstract bool IsDestination(Node node);

    public record Node(Point Position, Cost Cost) : IComparable<Node>
    {
        public int CompareTo(Node other)
        {
            return Cost.CompareTo(other.Cost);
        }
    }
        
    public record Cost(Point Parent, int TotalCost /* g(x) */) : IComparable<Cost>
    {
        public int CompareTo(Cost other)
        {
            return this.TotalCost.CompareTo(other.TotalCost);
        }
    }

    public record Point(int X, int Y);
}