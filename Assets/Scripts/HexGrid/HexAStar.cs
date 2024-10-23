using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
            this.GCost = Gcost;
            this.HCost = Hcost;
            this.Parent = null;
        }

        public Node(HexPoint point, float Gcost, float Hcost, Node parent)
        {
            this.point = point;
            this.GCost = Gcost;
            this.HCost = Hcost;
            this.Parent = parent;
        }

        public int CompareTo(Node other) => FCost.CompareTo(other.FCost);

        public override bool Equals(object obj)
        {
            if (obj is Node node)
            {
                return point == node.point;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return point.GetHashCode();
        }
    }

    private struct Directions {
        public static Vector3Int Right =       new Vector3Int(+1, 0,-1);
        public static Vector3Int TopRight =    new Vector3Int( 0,+1,-1);
        public static Vector3Int TopLeft =     new Vector3Int(-1,+1, 0);
        public static Vector3Int Left =        new Vector3Int(-1, 0,+1);
        public static Vector3Int BottomLeft =  new Vector3Int( 0,-1,+1);
        public static Vector3Int BottomRight = new Vector3Int(+1,-1, 0);

        public static Vector3Int Self = Vector3Int.zero;
    }

    private static readonly HexPoint topDirection =
        new HexPoint(Directions.TopRight + Directions.TopLeft, false);

    private static readonly HexPoint[] TopDirections = new HexPoint[]
    {
        new HexPoint(Directions.TopLeft, false),
        new HexPoint(Directions.Self, true),
        new HexPoint(Directions.TopRight, false)
    };

    private static readonly HexPoint TopRightDirection =
        new HexPoint(Directions.BottomRight, true);

    private static readonly HexPoint[] TopRightDirections = new HexPoint[]
    {
        new HexPoint(Directions.TopRight, true),
        new HexPoint(2 * Directions.TopRight, false),
        new HexPoint(Directions.Right, true)
    };

    private static readonly HexPoint TopLeftDirection =
        new HexPoint(Directions.BottomLeft, true);

    private static readonly HexPoint[] TopLeftDirections = new HexPoint[]
    {
        new HexPoint(Directions.Left, true),
        new HexPoint(2 * Directions.TopLeft, false),
        new HexPoint(Directions.TopLeft, true)
    };

    private static readonly HexPoint bottomDirection =
        new HexPoint(Directions.BottomRight + Directions.BottomLeft, true);

    private static readonly HexPoint[] BottomDirections = new HexPoint[]
    {
        new HexPoint(Directions.BottomLeft, true),
        new HexPoint(Directions.Self, false),
        new HexPoint(Directions.BottomRight, true)
    };

    private static readonly HexPoint BottomRightDirection =
        new HexPoint(Directions.TopRight, false);

    private static readonly HexPoint[] BottomRightDirections = new HexPoint[]
    {
        new HexPoint(Directions.BottomRight, false),
        new HexPoint(2 * Directions.BottomRight, true),
        new HexPoint(Directions.Right, false)
    };

    private static readonly HexPoint BottomLeftDirection =
        new HexPoint(Directions.TopLeft, false);

    private static readonly HexPoint[] BottomLeftDirections = new HexPoint[]
    {
        new HexPoint(Directions.Left, false),
        new HexPoint(2 * Directions.BottomLeft, true),
        new HexPoint(Directions.BottomLeft, false)
    };

    public static List<HexPoint> FindPath(HexPoint start, HexPoint goal, Tilemap groundMap, BuildingManager bm)
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
            if (currentNode.point.isTop)
            {
                Vector3Int bottomOffset = HexUtils.CubicToOffset(currentNode.point.cubicCoord);
                Vector3Int topRightOffset = HexUtils.CubicToOffset(currentNode.point.cubicCoord + Directions.TopRight);
                Vector3Int topLeftOffset = HexUtils.CubicToOffset(currentNode.point.cubicCoord + Directions.TopLeft);

                BasicTile bottomTile = groundMap.GetTile<BasicTile>(bottomOffset);
                BasicTile topRightTile = groundMap.GetTile<BasicTile>(topRightOffset);
                BasicTile topLeftTile = groundMap.GetTile<BasicTile>(topLeftOffset);

                TileType bottomType = bottomTile != null ? bottomTile.type : TileType.Empty;
                TileType topRightType = topRightTile != null ? topRightTile.type : TileType.Empty;
                TileType topLeftType = topLeftTile != null ? topLeftTile.type : TileType.Empty;

                Building bottomBuilding = bm.GetBuilding(bottomOffset);
                Building topRightBuilding = bm.GetBuilding(topRightOffset);
                Building topLeftBuilding = bm.GetBuilding(topLeftOffset);

                //condition for top path along edge
                if ((topRightType == TileType.Full || topLeftType == TileType.Full) &&
                    (topRightBuilding != topLeftBuilding || topRightBuilding == null))
                {
                    TraverseEdge(currentNode, topDirection, goal, openList, closedList);
                }

                //condition for bottom right path along edge
                if ((topRightType == TileType.Full || bottomType == TileType.Full) && 
                    (topRightBuilding != bottomBuilding || topRightBuilding == null)) 
                {
                    TraverseEdge(currentNode, BottomRightDirection, goal, openList, closedList);
                }

                //condition for bottom left path along edge
                if ((bottomType == TileType.Full || topLeftType == TileType.Full) && 
                    (bottomBuilding != topLeftBuilding || bottomBuilding == null))
                {
                    TraverseEdge(currentNode, BottomLeftDirection, goal, openList, closedList);
                }

                //condition for bottom paths through hexagon
                if (bottomType == TileType.Full && bottomBuilding == null)
                {
                    foreach (HexPoint direction in BottomDirections)
                    {
                        TraverseEdge(currentNode, direction, goal, openList, closedList);
                    }
                }

                //condition for top right paths through hexagon
                if (topRightType == TileType.Full && topRightBuilding == null)
                {
                    foreach (HexPoint direction in TopRightDirections)
                    {
                        TraverseEdge(currentNode, direction, goal, openList, closedList);
                    }
                }

                //condition for top left paths through hexagon
                if (topLeftType == TileType.Full && topLeftBuilding == null)
                {
                    foreach (HexPoint direction in TopLeftDirections)
                    {
                        TraverseEdge(currentNode, direction, goal, openList, closedList);
                    }
                }
            }
            else
            {
                Vector3Int topOffset = HexUtils.CubicToOffset(currentNode.point.cubicCoord);
                Vector3Int bottomRightOffset = HexUtils.CubicToOffset(currentNode.point.cubicCoord + Directions.BottomRight);
                Vector3Int bottomLeftOffset = HexUtils.CubicToOffset(currentNode.point.cubicCoord + Directions.BottomLeft);

                BasicTile topTile = groundMap.GetTile<BasicTile>(topOffset);
                BasicTile bottomRightTile = groundMap.GetTile<BasicTile>(bottomRightOffset);
                BasicTile bottomLeftTile = groundMap.GetTile<BasicTile>(bottomLeftOffset);

                TileType topType = topTile != null ? topTile.type : TileType.Empty;
                TileType bottomRightType = bottomRightTile != null ? bottomRightTile.type : TileType.Empty;
                TileType bottomLeftType = bottomLeftTile != null ? bottomLeftTile.type : TileType.Empty;

                Building topBuilding = bm.GetBuilding(topOffset);
                Building bottomRightBuilding = bm.GetBuilding(bottomRightOffset);
                Building bottomLeftBuilding = bm.GetBuilding(bottomLeftOffset);

                //condition for bottom path along edge
                if ((bottomRightType == TileType.Full || bottomLeftType == TileType.Full) &&
                    (bottomRightBuilding != bottomLeftBuilding || bottomRightBuilding == null))
                {
                    TraverseEdge(currentNode, bottomDirection, goal, openList, closedList);
                }

                //condition for top right path along edge
                if ((bottomRightType == TileType.Full || topType == TileType.Full) &&
                    (bottomRightBuilding != topBuilding || bottomRightBuilding == null))
                {
                    TraverseEdge(currentNode, TopRightDirection, goal, openList, closedList);
                }

                //condition for top left path along edge
                if ((topType == TileType.Full || bottomLeftType == TileType.Full) &&
                    (topBuilding != bottomLeftBuilding || topBuilding == null))
                {
                    TraverseEdge(currentNode, TopLeftDirection, goal, openList, closedList);
                }

                //condition for top paths through hexagon
                if (topType == TileType.Full && topBuilding == null)
                {
                    foreach (HexPoint direction in TopDirections)
                    {
                        TraverseEdge(currentNode, direction, goal, openList, closedList);
                    }
                }

                //condition for bottom right paths through hexagon
                if (bottomRightType == TileType.Full && bottomRightBuilding == null)
                {
                    foreach (HexPoint direction in BottomRightDirections)
                    {
                        TraverseEdge(currentNode, direction, goal, openList, closedList);
                    }
                }

                //condition for bottom left paths through hexagon
                if (bottomLeftType == TileType.Full && bottomLeftBuilding == null)
                {
                    foreach (HexPoint direction in BottomLeftDirections)
                    {
                        TraverseEdge(currentNode, direction, goal, openList, closedList);
                    }
                }
            }
        }

        // Return empty list if no path found
        return new List<HexPoint>();
    }

    private static void TraverseEdge(Node currentNode, HexPoint direction, HexPoint goal, SortedSet<Node> openList, HashSet<Node> closedList)
    {
        HexPoint neighborPoint = new HexPoint(currentNode.point.cubicCoord + direction.cubicCoord, direction.isTop);
        float GCost = currentNode.GCost + HexPoint.Distance(currentNode.point, neighborPoint);
        float HCost = HexPoint.Distance(neighborPoint, goal);
        Node neighborNode = new Node(neighborPoint, GCost, HCost, currentNode);

        if (!closedList.Contains(neighborNode) &&
            (!openList.TryGetValue(neighborNode, out Node existingNode) || GCost < existingNode.GCost))
        {
            openList.Add(neighborNode);
        }
    }

    private static List<HexPoint> ReconstructPath(Node endNode)
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
