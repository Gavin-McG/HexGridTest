using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HexUtils
{
    //translate offset coordinate to cubic coordinate
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3Int OffsetToCubic(in Vector2Int coords)
    {
        int x = coords.x - (coords.y - (coords.y & 1)) / 2;
        int z = -x - coords.y;
        return new Vector3Int(x,coords.y,z);
    }

    //translate offset coordinate to cubic coordinate
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3Int OffsetToCubic(in Vector3Int coords)
    {
        int x = coords.x - (coords.y - (coords.y & 1)) / 2;
        int z = -x - coords.y;
        return new Vector3Int(x,coords.y,z);
    }

    //translate cubic coordinate to offset coordinate
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3Int CubicToOffset(in Vector3Int coords)
    {
        int x = coords.x + (coords.y - (coords.y & 1)) / 2;
        return new Vector3Int(x,coords.y,0);
    }
}
