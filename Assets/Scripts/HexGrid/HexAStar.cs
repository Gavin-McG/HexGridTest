using System;
using System.Collections.Generic;
using UnityEngine;

public class HexAStar
{
    private class Node : IComparable<Node>
    {
        public HexPoint point;
        public float GCost;                     //Cost from start to current node
        public float HCost;                     //Heuristic cost (Euclidean distance)
        public float FCost => GCost + HCost;    //Total cost
        public Node Parent;                     //To reconstruct the path

        public Node(HexPoint point, float Gcost, float Hcost)
        {
            this.point = point;
        }

        public int CompareTo(Node other) => FCost.CompareTo(other.FCost);
    }

    private struct Directions {
        public static Vector3 Right =       new Vector3(+1, 0,-1);
        public static Vector3 TopRight =    new Vector3( 0,+1,-1);
        public static Vector3 TopLeft =     new Vector3(-1,+1, 0);
        public static Vector3 Left =        new Vector3(-1, 0,+1);
        public static Vector3 BottomLeft =  new Vector3( 0,-1,+1);
        public static Vector3 BottomRight = new Vector3(+1,-1, 0);

        public static Vector3 Self = Vector3.zero;
    }

    private static readonly (Vector3, bool) topDirection = 
        (Directions.TopRight + Directions.TopLeft, false);
    private static readonly (Vector3, bool)[] TopDirections = new (Vector3, bool)[]
    {
        (Directions.TopLeft,    false),    
        (Directions.Self,       true),
        (Directions.TopRight,   false)
    };

    private static readonly (Vector3, bool) TopRightDirection =
        (Directions.BottomRight, true);
    private static readonly (Vector3, bool)[] TopRightDirections = new (Vector3, bool)[]
    {
        (Directions.TopRight,   true),
        (2*Directions.TopRight, false),
        (Directions.Right,      true)
    };

    private static readonly (Vector3, bool) TopLeftDirection =
        (Directions.BottomLeft, true);
    private static readonly (Vector3, bool)[] TopLeftDirections = new (Vector3, bool)[]
    {
        (Directions.Left,      true),
        (2*Directions.TopLeft, false),
        (Directions.TopLeft,   true)
    };

    private static readonly (Vector3, bool) bottomDirection =
        (Directions.BottomRight + Directions.BottomLeft, true);
    private static readonly (Vector3, bool)[] BottomDirections = new (Vector3, bool)[]
    {
        (Directions.BottomLeft,     true),
        (2*Directions.Self,         false),
        (Directions.BottomRight,    true)
    };

    private static readonly (Vector3, bool) BottomRightDirection =
        (Directions.TopRight, false);
    private static readonly (Vector3, bool)[] BottomRightDirections = new (Vector3, bool)[]
    {
        (Directions.BottomRight,    false),
        (2*Directions.BottomRight,  true),
        (Directions.Right,          false)
    };

    private static readonly (Vector3, bool) BottomLeftDirection =
        (Directions.TopLeft, false);
    private static readonly (Vector3, bool)[] BottomLeftDirections = new (Vector3, bool)[]
    {
        (Directions.Left,           false),
        (2*Directions.BottomLeft,   true),
        (Directions.BottomLeft,     false)
    };

    public List<HexPoint> FindPath(HexPoint start, HexPoint goal)
    {
        // Initialize open and closed lists (open = nodes to explore, closed = already explored)
        var openList = new SortedSet<Node>();
        var closedList = new HashSet<Node>();

        var startNode = new Node(start, 0, HexPoint.Distance(start, goal));
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // Get the node with the lowest FCost
            Node currentNode = openList.Min;
            openList.Remove(currentNode);

            // Check if we've reached the goal
            if (currentNode.point == goal)
                return ReconstructPath(currentNode);

            closedList.Add(currentNode);

            // Explore each neighbor
            
        }

        // Return empty list if no path found
        return new List<HexPoint>();
    }

    private List<HexPoint> ReconstructPath(Node endNode)
    {
        var path = new List<HexPoint>();
        Node currentNode = endNode;

        while (currentNode != null)
        {
            path.Add(currentNode.point);
            currentNode = currentNode.Parent;
        }

        path.Reverse(); // Path was built backward, so reverse it
        return path;
    }
}
