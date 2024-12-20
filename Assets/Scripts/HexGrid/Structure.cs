using UnityEngine;

//defines the structure of tiles for a type of building

[System.Serializable]
public struct StructurePiece
{
    public BasicTile tile;
    public Vector3Int cubicCoord;
}

[CreateAssetMenu(fileName = "Structure", menuName = "ScriptableObjects/Structure", order = 1)]
public class Structure : ScriptableObject
{
    public GameObject buildingObject;
    public StructurePiece[] pieces;
}
