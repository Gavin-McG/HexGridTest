using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CoordinateTest : MonoBehaviour
{
    [SerializeField] Tilemap map;
    [SerializeField] Tile tile;

    // Start is called before the first frame update
    void Start()
    {
        for (int i=-5; i<=5; i++)
        {
            map.SetTile(HexUtils.CubicToOffset(new Vector3Int(i,-i,0)), tile);
            map.SetTile(HexUtils.CubicToOffset(new Vector3Int(i,0,-i)), tile);
            map.SetTile(HexUtils.CubicToOffset(new Vector3Int(0,i,-1)), tile);
        }
    }
}
