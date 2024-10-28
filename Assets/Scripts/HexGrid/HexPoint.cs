using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class HexPoint : IComparable<HexPoint>
{
    public Vector3Int cubicCoord;
    public bool isTop;

    public HexPoint(Vector3Int cubicCoord, bool isTop)
    {
        this.cubicCoord = cubicCoord;
        this.isTop = isTop;
    }

    public static readonly float Sqrt3Over2 = Mathf.Sqrt(3) / 2;

    public Vector3 getPosition()
    {
        //calculate center of hexagon from
        Vector3 hexCenter = new Vector3(
            Sqrt3Over2 * (cubicCoord.x - cubicCoord.z),
            1.5f * cubicCoord.y,
            0f
        );

        //offset for top/bottom point
        hexCenter.y += isTop ? +1 : -1;

        return hexCenter;
    }

    public Vector3 getPosition(Tilemap map)
    {
        Vector3 pos = map.CellToLocal(HexUtils.CubicToOffset(cubicCoord));
        pos += Vector3.up * ((isTop ? +1 : -1) * map.cellSize.y / 2 + 0.11f);
        return pos;
        
    }

    public static float Distance(HexPoint p1, HexPoint p2)
    {
        //get positions
        Vector3 p1Pos = p1.getPosition();
        Vector3 p2Pos = p2.getPosition();

        //calauclate distance
        return (p1Pos - p2Pos).magnitude;
    }

    public static bool operator ==(HexPoint p1, HexPoint p2)
    {
        if (p1 is null && p2 is null) return true;
        if (p1 is null || p2 is null) return false;
        return p1.cubicCoord == p2.cubicCoord && p1.isTop == p2.isTop;
    }

    public static bool operator !=(HexPoint p1, HexPoint p2)
    {
        return p1.cubicCoord != p2.cubicCoord || p1.isTop != p2.isTop;
    }

    public override bool Equals(object obj)
    {
        if (obj is HexPoint node)
        {
            return this == node;
        }
        return false;
    }

    public override int GetHashCode()
    {
        // Generate a hash code based on the coordinates
        return (cubicCoord, isTop).GetHashCode();
    }

    public int CompareTo(HexPoint other)
    {
        if (other == null)
            return 1;

        // First, compare Vector3Int components (x, y, z)
        int compareX = cubicCoord.x.CompareTo(other.cubicCoord.x);
        if (compareX != 0) return compareX;

        int compareY = cubicCoord.y.CompareTo(other.cubicCoord.y);
        if (compareY != 0) return compareY;

        int compareZ = cubicCoord.z.CompareTo(other.cubicCoord.z);
        if (compareZ != 0) return compareZ;

        // If Vector3Int components are equal, compare the bool isActive
        return isTop.CompareTo(other.isTop);
    }

    public override string ToString()
    {
        return cubicCoord.ToString() + " " + (isTop?"Top":"Bottom");
    }
}
