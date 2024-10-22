using System;
using System.Collections.Generic;
using UnityEngine;

public class HexAStar
{
    private class Node : IComparable<Node>
    {
        public Vector4 coords;
        public float GCost;                     //Cost from start to current node
        public float HCost;                     //Heuristic cost (Euclidean distance)
        public float FCost => GCost + HCost;    //Total cost
        public Node Parent;                     //To reconstruct the path

        public Node(Vector4 _coords)
        {
            coords = _coords;
        }

        public int CompareTo(Node other) => FCost.CompareTo(other.FCost);
    }
    
}
