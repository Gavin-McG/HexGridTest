using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Building Spawn Tile", menuName = "Tiles/Building Spawn Tile")]
public class BuildingSpawnTile : Tile
{
    public Building building;
}
