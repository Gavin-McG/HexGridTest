using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Environment Tile", menuName = "Tiles/Environment Tile")]
public class EnvironmentTile : BasicTile
{
    [Space(10)]
    [SerializeField] public string tileName;
}
