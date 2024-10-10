using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingManager : MonoBehaviour
{
    //tilemaps used for building processes
    [SerializeField] Tilemap groundMap;
    [SerializeField] Tilemap objectMap;
    [SerializeField] Tilemap previewMap;

    [Space(10)]

    //colors to reresent valid/invalid placement
    [SerializeField] Color validColor = Color.green;
    [SerializeField] Color invalidColor = Color.red;

    [Space(10)]

    //current build mode state
    [SerializeField] bool buildMode = true;
    [SerializeField] Structure activeStructure;

    //dictionaries to track buildings
    //all Vector3Int of dictionaries are stored in Offset coordinates

    //tileDictionary tracks what Building each tile correlates to.
    //When placing a building all tiles the building fills should have their value set
    Dictionary<Vector3Int, Building> tileDictionary;
    //buildingDictionary tracks the tiles that are possessed by each building.
    //When placing a building all tiles the building fills should be added into its value list
    Dictionary<Building, List<Vector3Int>> buildingDictionary;
    //typeDictionary manages a list of all buildings of each type
    Dictionary<BuildingType, List<Building>> typeDictionary;


    private void Start()
    {
        //initialize dictionaries
        tileDictionary = new Dictionary<Vector3Int, Building>();
        buildingDictionary = new Dictionary<Building, List<Vector3Int>>();
        typeDictionary = new Dictionary<BuildingType, List<Building>>();
    }

    void Update()
    {
        if (buildMode)
        {
            CreatePreview();

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPosition.z = 0;
                Vector3Int offsetCoord = groundMap.WorldToCell(worldPosition);

                PlaceBuilding(offsetCoord, activeStructure);
            }
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

    //check whether a given offset position is a valid place for a tile to be set
    public bool IsValidPlacement(Vector3Int offsetCoord)
    {
        //get tiles in the offset position
        TileBase groundTile = groundMap.GetTile(offsetCoord);
        TileBase objectTile = objectMap.GetTile(offsetCoord);

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

    //check whether a given offset position is a valid place for a structure to be set
    public bool IsValidStructure(Vector3Int offsetCoord, Structure structure)
    {
        if (structure == null) return false;

        Vector3Int cubicCoord = HexUtils.OffsetToCubic(offsetCoord);
        foreach (StructurePeice peice in structure.peices)
        {
            //calculate coordinate
            Vector3Int newCubicCoord = cubicCoord + peice.cubicCoord;
            Vector3Int newOffsetCoord = HexUtils.CubicToOffset(newCubicCoord);

            //check peice's valid placement
            if (!IsValidPlacement(newOffsetCoord)) return false;
        }

        return true;
    }


    public bool PlaceBuilding(Vector3Int offsetCoord, Structure structure)
    {
        //skip if structure is null
        if (!IsValidStructure(offsetCoord, structure)) return false;

        //create new building
        Building newBuilding = Building.GetBuilding(structure.buildingType);

        //add new building to building dictionary
        buildingDictionary.Add(newBuilding, new List<Vector3Int>());

        //add new building to type dictionary
        if (!typeDictionary.ContainsKey(structure.buildingType))
            //add new key if necessary of buildingType
            typeDictionary.Add(structure.buildingType, new List<Building>());
        typeDictionary[structure.buildingType].Add(newBuilding);

        //place all tiles of structure
        Vector3Int cubicCoord = HexUtils.OffsetToCubic(offsetCoord);
        foreach (StructurePeice peice in structure.peices)
        {
            //calculate coordinate
            Vector3Int newCubicCoord = cubicCoord + peice.cubicCoord;
            Vector3Int newOffsetCoord = HexUtils.CubicToOffset(newCubicCoord);

            //set tile
            objectMap.SetTile(newOffsetCoord, peice.tile);

            //put tile in tileDictionary
            if (!tileDictionary.ContainsKey(newOffsetCoord))
                //set new tile's value as newBuilding
                tileDictionary.Add(newOffsetCoord, newBuilding);
            else
                //set tile's value as newBuilding
                tileDictionary[newOffsetCoord] = newBuilding;

            //put tile in buildingDictionary
            buildingDictionary[newBuilding].Add(newOffsetCoord);
        }

        return true;
    }
}
