using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


//Custom Tile used to store needed information for each tile


//types of tiles needed for terrain info
[System.Serializable]
public enum TileType
{
    Empty,
    Full,
}


[CreateAssetMenu(fileName = "New Basic Tile", menuName = "Tiles/Basic Tile")]
public class BasicTile : Tile
{
    [Space(20)] 
    public TileType type;
}