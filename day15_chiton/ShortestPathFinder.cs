using System;
using System.Collections.Generic;

namespace day15_chiton;

public class ShortestPathFinder : AStar
{
    public Node Solution;
    private int[,] Map;
    private int Dim;
    private Point Destination;
    private Dictionary<Point, Cost> ClosedList;
    private PriorityQueue<Node, int> OpenList;

    public ShortestPathFinder(int[,] map, int dim)
    {
        Map = map;
        Dim = dim;
        Destination = new Point(Dim - 1, Dim - 1);
        ClosedList = new Dictionary<Point, Cost>();
        OpenList = new PriorityQueue<Node, int>();
        var startNode = new Node(new Point(0, 0), new Cost(new Point(0, 0), 0));
        FindPathFrom(startNode, OpenList, ClosedList);
    }

    protected override void AddNeighbours(Node node, PriorityQueue<Node, int> openList)
    {
        for (int dx = -1; dx <= 1; dx++)
        for (int dy = -1; dy <= 1; dy++)
            if (!(dx == 0 && dy == 0) && !(Math.Abs(dx) + Math.Abs(dy) > 1)) // exclude self and diagonal
            {
                var newPos = new Point(node.Position.X + dx, node.Position.Y + dy);
                if (newPos.X >= 0 && newPos.X < Dim && newPos.Y >= 0 && newPos.Y < Dim)
                {
                    int distanceCost = node.Cost.TotalCost + GetCost(newPos);
                    openList.Enqueue(new Node(newPos, new Cost(node.Position, distanceCost)), distanceCost);
                }
            }
    }

    private int GetCost(Point point)
    {
        return Map[point.X, point.Y];
    }

    protected override bool IsDestination(Node node)
    {
        bool isSolved = node.Position == Destination;
        if (isSolved)
            Solution = node;
        return isSolved;
    }
}