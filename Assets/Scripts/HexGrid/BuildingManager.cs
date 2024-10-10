using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public enum EditMode
{
    None,
    Build,
    Delete
} 


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
    [SerializeField] EditMode _editMode = EditMode.None;
    [SerializeField] Structure activeStructure;

    //readonly parameter for _editMode
    [HideInInspector] public EditMode editMode {get {return _editMode;}}

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


    //UI events to change edit mode (might move to UI scripts later)
    public static UnityEvent<Structure> EnableBuilding = new UnityEvent<Structure>();
    public static UnityEvent EnableDeleting = new UnityEvent();
    public static UnityEvent DisableEditing = new UnityEvent();

    //events to mark building changes
    public static UnityEvent<Building> BuildingPlaced = new UnityEvent<Building>();
    public static UnityEvent<Building> BuildingDeleted = new UnityEvent<Building>();


    void Awake()
    {
        //initialize dictionaries
        tileDictionary = new Dictionary<Vector3Int, Building>();
        buildingDictionary = new Dictionary<Building, List<Vector3Int>>();
        typeDictionary = new Dictionary<BuildingType, List<Building>>();
    }

    private void OnEnable()
    {
        EnableBuilding.AddListener(SetBuildMode);
        EnableDeleting.AddListener(SetDeleteMode);
        DisableEditing.AddListener(SetNoneMode);
    }

    private void OnDisable()
    {
        EnableBuilding.RemoveListener(SetBuildMode);
        EnableDeleting.RemoveListener(SetDeleteMode);
        DisableEditing.RemoveListener(SetNoneMode);
    }

    void Update()
    {
        if (_editMode == EditMode.Build)
        {
            Vector3Int offsetCoord = GetSelectedOffset();

            CreatePreview(offsetCoord, activeStructure);

            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuilding(offsetCoord, activeStructure);
            }
        }
        else if (_editMode == EditMode.Delete)
        {
            Vector3Int offsetCoord = GetSelectedOffset();

            if (Input.GetMouseButtonDown(0))
            {
                DeleteBuilding(offsetCoord);
            }
        }
    }




    //Get offset position of mouse position
    public Vector3Int GetSelectedOffset()
    {
        //set world position of mouse
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0;

        //convert world position to offset position
        Vector3Int offsetCoord = groundMap.WorldToCell(worldPosition);
        
        return offsetCoord;
    }


    //Dsiplay preview of structure to be placed, changes color based on placement validity
    void CreatePreview(Vector3Int offsetCoord, Structure structure)
    {
        if (!Application.isFocused) return;

        //clear preview map
        previewMap.ClearAllTiles();

        Vector3Int cubicCoord = HexUtils.OffsetToCubic(offsetCoord);
        foreach (StructurePeice peice in structure.peices)
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




    //Turn on Build mode, required provided structure
    public void SetBuildMode(Structure structure)
    {
        _editMode = EditMode.Build;
        activeStructure = structure;
    }

    //Turn on Delete mode
    public void SetDeleteMode()
    {
        _editMode = EditMode.Delete;
        previewMap.ClearAllTiles();
    }

    //disable editing modes
    public void SetNoneMode()
    {
        _editMode = EditMode.Build;
        previewMap.ClearAllTiles();
    }




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




    //place a structure at given offsetCoords
    //return true is placement is successful
    public bool PlaceBuilding(Vector3Int offsetCoord, Structure structure, bool placeEvent = true)
    {
        //skip if structure placement isn't valid
        if (!IsValidStructure(offsetCoord, structure)) return false;

        //create new building
        Building newBuilding = Building.GetBuilding(structure.buildingType);

        //add new building to type dictionary
        if (!typeDictionary.ContainsKey(structure.buildingType))
            //add new key if necessary of buildingType
            typeDictionary.Add(structure.buildingType, new List<Building>());
        typeDictionary[structure.buildingType].Add(newBuilding);

        //add new building to building dictionary
        buildingDictionary.Add(newBuilding, new List<Vector3Int>());

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

        //run building place event
        if (placeEvent)
        {
            BuildingPlaced.Invoke(newBuilding);
        }

        return true;
    }




    //delete a structure at given offsetCoords
    //return true is deletion is successful
    public bool DeleteBuilding(Vector3Int offsetCoords, bool deleteEvent = true)
    {
        //get building from tile
        if (!tileDictionary.ContainsKey(offsetCoords)) return false;
        Building building = tileDictionary[offsetCoords];

        //remove building from typeDictionary
        typeDictionary[building.type].Remove(building);

        //loop through tiles in building
        foreach (Vector3Int tileOffset in buildingDictionary[building])
        {
            //delete tile from objectMap
            objectMap.SetTile(tileOffset, null);

            //remove tile from tileDictionary
            tileDictionary.Remove(tileOffset);
        }

        //remove from buildingDictionary
        buildingDictionary.Remove(building);

        //run building delete event
        if (deleteEvent)
        {
            BuildingDeleted.Invoke(building);
        }

        return true;
    }
}
