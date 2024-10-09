using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] Tilemap groundMap;
    [SerializeField] Tilemap objectMap;
    [SerializeField] Tilemap previewMap;

    [Space(10)]

    [SerializeField] Color validColor = Color.green;
    [SerializeField] Color invalidColor = Color.red;

    [Space(10)]

    [SerializeField] bool buildMode = true;
    public Structure activeStructure;

    void Update()
    {
        if (buildMode)
        {
            CreatePreview();
        }
    }

    void CreatePreview()
    {
        if (!Application.isFocused) return;

        //get highlighted coordinate
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0;
        Vector3Int offsetCoord = groundMap.WorldToCell(worldPosition);
        Vector3Int cubicCoord = HexUtils.OffsetToCubic(offsetCoord);

        //clear preview map
        previewMap.ClearAllTiles();

        foreach (StructurePeice peice in activeStructure.peices)
        {
            //calculate coordinate
            Vector3Int newCubicCoord = cubicCoord + peice.cubicCoord;
            Vector3Int newOffsetCoord = HexUtils.CubicToOffset(newCubicCoord);

            //set tile
            previewMap.SetTile(newOffsetCoord, peice.tile);
            bool isValid = IsValidPlacement(newOffsetCoord);
            previewMap.SetTileFlags(newOffsetCoord, TileFlags.None);
            previewMap.SetColor(newOffsetCoord, isValid ? validColor : invalidColor);
        }
    }

    public void EnableBuildMode(Structure structure)
    {
        buildMode = true;
        activeStructure = structure;
    }

    public void DisableBuildMode()
    {
        buildMode = false;
        previewMap.ClearAllTiles();
    }

    public bool IsBuildMode() { return buildMode; }

    public bool IsValidPlacement(Vector3Int offsetPosition)
    {
        //get tiles in the offset position
        TileBase groundTile = groundMap.GetTile(offsetPosition);
        TileBase objectTile = objectMap.GetTile(offsetPosition);

        //default empty types
        TileType groundType = TileType.Empty;
        TileType objectType = TileType.Empty;

        //retreive types of tiles if valid tile/not null
        if (groundTile is CustomTile groundHex) 
        {
            groundType = groundHex.type;
        }
        if (objectTile is CustomTile objectHex)
        {
            objectType = objectHex.type;
        }

        //placeable is ground is full and object is empty
        return groundType==TileType.Full && objectType==TileType.Empty;
    } 

}
