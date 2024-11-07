using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnvironmentType
{
    Tree
}

[CreateAssetMenu(fileName = "New Environment Tile", menuName = "Tiles/Environment Tile")]
public class EnvironmentTile : BasicTile
{
    [Space(10)]
    [SerializeField] EnvironmentType envType;
    [SerializeField] public string tileName;
    [SerializeField] public bool isTree;
}
