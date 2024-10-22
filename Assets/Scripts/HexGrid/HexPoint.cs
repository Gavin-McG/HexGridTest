using Unity.VisualScripting;
using UnityEngine;

public class HexPoint
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

    public static float Distance(HexPoint p1, HexPoint p2)
    {
        //get positions
        Vector3 p1Pos = p1.getPosition();
        Vector3 p2Pos = p2.getPosition();

        //calauclate distance
        return (p1Pos - p2Pos).magnitude;
    }

    public HexPoint SetTop(bool isTop)
    {
        this.isTop = isTop;
        return this;
    }

    public static HexPoint operator +(HexPoint p1, Vector3Int cubicCoord) {
        return new HexPoint(p1.cubicCoord + cubicCoord, p1.isTop);
    }

    public static HexPoint operator -(HexPoint p1, Vector3Int cubicCoord)
    {
        return new HexPoint(p1.cubicCoord - cubicCoord, p1.isTop);
    }
}