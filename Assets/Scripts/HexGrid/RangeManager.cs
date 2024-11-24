using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RangeManager : MonoBehaviour
{
    public static RangeManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    [SerializeField] BuildingManager bm;
    [SerializeField] Tilemap rangeMap;
    [SerializeField] Tilemap farmRangeMap;
    [SerializeField] TileBase highlightTile;


    //runs if the checking is already active to stop recursion issues for optimization purposes
    bool checking = false;

    private void OnEnable()
    {
        BuildingManager.BuildingPlaced.AddListener(PlaceRange);
        BuildingManager.BuildingDeleted.AddListener(RemoveRange);
        
        BuildingManager.BuildingClicked.AddListener(EnableFarmRange);
        BuildingManager.EnvironmentClicked.AddListener(DisableFarmRange);

        //listen for editMode updates
        BuildingManager.EnableBuilding.AddListener(EnableRange);
        BuildingManager.EnableDeleting.AddListener(EnableRange);
        BuildingManager.DisableEditing.AddListener(DisableRange);
    }

    private void OnDisable()
    {
        BuildingManager.BuildingPlaced.RemoveListener(PlaceRange);
        BuildingManager.BuildingDeleted.RemoveListener(RemoveRange);

        BuildingManager.EnableBuilding.RemoveListener(EnableRange);
        BuildingManager.EnableDeleting.RemoveListener(EnableRange);
        BuildingManager.DisableEditing.RemoveListener(DisableRange);
    }


    void PlaceRange(Building building, Vector3Int offsetCoord)
    {
        //get building range
        int range = 0;
        //Changes PlaceRing logic
        bool isFarm = false;
        if (building is MainTower mainTower)
        {
            range = mainTower.buildRange;
        }
        else if (building is WizardTower tower)
        {
            range = tower.buildRange;
        }
        else if (building is Farm farm)
        {
            range = farm.range;
            isFarm = true;
        }

        if (range == 0) return;

        //place tiles
        for (int i=0; i<=range; i++)
        {
            PlaceRing(i, HexUtils.OffsetToCubic(building.offsetCoord), isFarm);
        }
    }

    void PlaceRing(int radius, Vector3Int centerCubic, bool isFarm)
    {
        if (radius < 0) return;

        //set single tile for radius=0
        if (radius == 0)
        {
            Vector3Int offsetCoord = HexUtils.CubicToOffset(centerCubic);
            if (isFarm)
            {
                farmRangeMap.SetTile(offsetCoord, highlightTile);
                AdjustBuildingProduction(offsetCoord, increase: true);
            }
            else { rangeMap.SetTile(offsetCoord, highlightTile); }
            return;
        }

        // Starting position in cubic coordinates (on the positive x-axis of the ring)
        Vector3Int currentCubic = new Vector3Int(radius, -radius, 0);

        // Array representing six directions to traverse the ring for pointed-top hexagons
        Vector3Int[] directions = new Vector3Int[]
        {
            new Vector3Int(0, 1, -1),   // Top-right
            new Vector3Int(-1, 1, 0),   // Top-left
            new Vector3Int(-1, 0, 1),   // Left
            new Vector3Int(0, -1, 1),   // Bottom-left
            new Vector3Int(1, -1, 0),   // Bottom-right
            new Vector3Int(1, 0, -1)    // Right
        };

        // Place tiles along the hexagonal ring
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < radius; j++)
            {
                // Convert current cubic position to offset coordinates
                Vector3Int offsetPosition = HexUtils.CubicToOffset(currentCubic + centerCubic);

                // Place the tile at the calculated offset position
                if (bm.IsGroundTile(offsetPosition))
                {
                    if (isFarm)
                    {
                        farmRangeMap.SetTile(offsetPosition, highlightTile);
                        AdjustBuildingProduction(offsetPosition, increase: true);
                    }
                    else { rangeMap.SetTile(offsetPosition, highlightTile); }
                }

                // Move to the next position in the current direction
                currentCubic += directions[i];
            }
        }
    }

    void RemoveRange(Building building, Vector3Int offsetCoord)
    {
        //get building range
        int range = 0;
        if (building is MainTower mainTower)
        {
            range = mainTower.buildRange;
        }
        else if (building is WizardTower tower)
        {
            range = tower.buildRange;
        }
        else if (building is Farm farm)
        {
            range = farm.range;
        }

        if (range == 0) return;

        if (building is Farm)
        {
            //get coords and radii of other towers
            List<Vector3Int> cubicCoords = new List<Vector3Int>();
            List<int> radii = new List<int>();

            List<Farm> farms = bm.GetBuildingsOfType(BuildingType.Farm).Cast<Farm>().ToList();
            for (int i = 0; i < farms.Count; i++)
            {
                cubicCoords.Add(HexUtils.OffsetToCubic(farms[i].offsetCoord));
                radii.Add(farms[i].range);
            }

            for (int i = 0; i <= range; i++)
            {
                RemoveFarmRing(i, HexUtils.OffsetToCubic(building.offsetCoord), cubicCoords, radii);
            }
        }
        else
        {
            //get coords and radii of other towers
            List<Vector3Int> cubicCoords = new List<Vector3Int>();
            List<int> radii = new List<int>();

            List<MainTower> mainTowers = bm.GetBuildingsOfType(BuildingType.MainTower).Cast<MainTower>().ToList();
            for (int i = 0; i < mainTowers.Count; i++)
            {
                cubicCoords.Add(HexUtils.OffsetToCubic(mainTowers[i].offsetCoord));
                radii.Add(mainTowers[i].buildRange);
            }

            List<WizardTower> wizardTowers = bm.GetBuildingsOfType(BuildingType.WizardTower).Cast<WizardTower>().ToList();
            for (int i = 0; i < wizardTowers.Count; i++)
            {
                cubicCoords.Add(HexUtils.OffsetToCubic(wizardTowers[i].offsetCoord));
                radii.Add(wizardTowers[i].buildRange);
            }

            //remove tiles
            for (int i = 0; i <= range; i++)
            {
                RemoveRing(i, HexUtils.OffsetToCubic(offsetCoord), cubicCoords, radii);
            }

            //update ranges
            if (!checking)
            {
                CleanTowers();
            }
        }
    }

    void RemoveRing(int radius, Vector3Int centerCubic, List<Vector3Int> cubicCoords, List<int> radii)
    {
        if (radius < 0) return;

        //set single tile for radius=0
        if (radius == 0)
        {
            RemoveTile(centerCubic, cubicCoords, radii);
            return;
        }

        // Starting position in cubic coordinates (on the positive x-axis of the ring)
        Vector3Int currentCubic = new Vector3Int(radius, -radius, 0);

        // Array representing six directions to traverse the ring for pointed-top hexagons
        Vector3Int[] directions = new Vector3Int[]
        {
            new Vector3Int(0, 1, -1),   // Top-right
            new Vector3Int(-1, 1, 0),   // Top-left
            new Vector3Int(-1, 0, 1),   // Left
            new Vector3Int(0, -1, 1),   // Bottom-left
            new Vector3Int(1, -1, 0),   // Bottom-right
            new Vector3Int(1, 0, -1)    // Right
        };

        // Place tiles along the hexagonal ring
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < radius; j++)
            {
                // Convert current cubic position to offset coordinates
                Vector3Int cubicPosition = currentCubic + centerCubic;

                // Place the tile at the calculated offset position
                RemoveTile(cubicPosition, cubicCoords, radii);

                // Move to the next position in the current direction
                currentCubic += directions[i];
            }
        }
    }

    void RemoveFarmRing(int radius, Vector3Int centerCubic, List<Vector3Int> cubicCoords, List<int> radii)
    {
        if (radius < 0) return;

        if (radius == 0)
        {
            RemoveFarmTile(centerCubic, cubicCoords, radii);
            return;
        }

        Vector3Int currentCubic = new Vector3Int(radius, -radius, 0);
        
        Vector3Int[] directions = new Vector3Int[]
        {
            new Vector3Int(0, 1, -1),   // Top-right
            new Vector3Int(-1, 1, 0),   // Top-left
            new Vector3Int(-1, 0, 1),   // Left
            new Vector3Int(0, -1, 1),   // Bottom-left
            new Vector3Int(1, -1, 0),   // Bottom-right
            new Vector3Int(1, 0, -1)    // Right
        };

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < radius; j++)
            {
                Vector3Int cubicPosition = currentCubic + centerCubic;
                RemoveFarmTile(cubicPosition, cubicCoords, radii);
                currentCubic += directions[i];
            }
        }
    }

    void RemoveTile(Vector3Int tileCoord, List<Vector3Int> cubicCoords, List<int> radii)
    {
        for (int i = 0; i<cubicCoords.Count; i++)
        {
            if (HexUtils.CubicDIstance(tileCoord, cubicCoords[i]) <= radii[i]) return;
        }
        
        //remove tile from range
        Vector3Int offsetCoord = HexUtils.CubicToOffset(tileCoord);
        rangeMap.SetTile(offsetCoord, null);

        //remove building at tile
        Building building = bm.GetBuilding(offsetCoord);
        if (building != null)
        {
            bm.DeleteBuilding(offsetCoord, true);
        }
    }

    void RemoveFarmTile(Vector3Int cubicCoord, List<Vector3Int> cubicCoords, List<int> radii)
    {
        for (int i = 0; i < cubicCoords.Count; i++)
        {
            if (HexUtils.CubicDIstance(cubicCoord, cubicCoords[i]) <= radii[i]) return;
        }

        Vector3Int offsetCoord = HexUtils.CubicToOffset(cubicCoord);
        farmRangeMap.SetTile(offsetCoord, null);
        AdjustBuildingProduction(offsetCoord, increase: false);
    }

    void AdjustBuildingProduction(Vector3Int offsetCoord, bool increase)
    {
        Building building = bm.GetBuilding(offsetCoord);
        if (building != null && building.canProduce)
        {
            if (increase) { building.IncreaseProduction(); }
            else { building.RevertProduction(); }
        }
    }


    enum CheckState
    {
        NotFound,
        Queued,
        Checked
    }

    class TowerEntry
    {
        public Vector3Int cubicCoord;
        public int range;
        public CheckState state;

        public TowerEntry (Vector3Int cubicCoord, int range, CheckState state)
        {
            this.cubicCoord = cubicCoord;
            this.range = range;
            this.state = state;
        }
    }

    //check that all ranges can chain to main tower. Remove tower otherwise.
    void CleanTowers()
    {
        checking = true;

        //init list
        List<TowerEntry> towerData = new List<TowerEntry>();

        //add main tower to list
        List<MainTower> mainTowers = bm.GetBuildingsOfType(BuildingType.MainTower).Cast<MainTower>().ToList();
        towerData.Add(new TowerEntry(HexUtils.OffsetToCubic(mainTowers[0].offsetCoord), mainTowers[0].buildRange, CheckState.Queued));

        //add wizard towers to list
        List<WizardTower> wizardTowers = bm.GetBuildingsOfType(BuildingType.WizardTower).Cast<WizardTower>().ToList();
        foreach (WizardTower tower in wizardTowers)
        {
            towerData.Add(new TowerEntry(HexUtils.OffsetToCubic(tower.offsetCoord), tower.buildRange, CheckState.NotFound));
        }

        //run checking algorithm
        bool finished = false;
        while (!finished)
        {
            finished = true;
            for (int i = 0; i<towerData.Count; ++i)
            {
                if (towerData[i].state == CheckState.Queued)
                {
                    finished = false;
                    for (int j=1; j<towerData.Count; ++j)
                    {
                        int distance = HexUtils.CubicDIstance(towerData[i].cubicCoord, towerData[j].cubicCoord);
                        if (towerData[j].state == CheckState.NotFound && distance <= towerData[i].range)
                        {
                            towerData[j].state = CheckState.Queued;
                        }
                    }
                    towerData[i].state = CheckState.Checked;
                    break;
                }
            }
        }

        //remove all notfound towers
        for (int i = 0; i<towerData.Count; ++i)
        {
            if (towerData[i].state == CheckState.NotFound)
            {
                bm.DeleteBuilding(HexUtils.CubicToOffset(towerData[i].cubicCoord));
            }
        }

        checking = false;
    }

    public List<Vector3Int> GetFarmRangeTiles(Farm farm)
    {
        List<Vector3Int> rangeTiles = new List<Vector3Int>();
        int range = farm.range;

        Vector3Int centerCubic = HexUtils.OffsetToCubic(farm.offsetCoord);

        for (int radius = 0; radius <= range; radius++)
        {
            Vector3Int[] directions = new Vector3Int[]
            {
                new Vector3Int(0, 1, -1),   // Top-right
                new Vector3Int(-1, 1, 0),   // Top-left
                new Vector3Int(-1, 0, 1),   // Left
                new Vector3Int(0, -1, 1),   // Bottom-left
                new Vector3Int(1, -1, 0),   // Bottom-right
                new Vector3Int(1, 0, -1)    // Right
            };
            Vector3Int currentCubic = new Vector3Int(radius, -radius, 0);

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < radius; j++)
                {
                    Vector3Int offsetCoord = HexUtils.CubicToOffset(centerCubic + currentCubic);
                    if (farmRangeMap.HasTile(offsetCoord))
                    {
                        rangeTiles.Add(offsetCoord);
                    }

                    currentCubic += directions[i];
                }
            }
        }

        return rangeTiles;
    }

    void EnableRange()
    {
        rangeMap.gameObject.SetActive(true);
    }

    void EnableRange(Structure structure)
    {
        rangeMap.gameObject.SetActive(true);
    }

    void EnableFarmRange(Building building, Vector3Int offsetCoord)
    {
        if (building.type is BuildingType.Farm) { farmRangeMap.gameObject.SetActive(true); }
        else { farmRangeMap.gameObject.SetActive(false); }
    }

    void DisableRange()
    {
        rangeMap.gameObject.SetActive(false);
    }

    void DisableFarmRange(EnvironmentTile tile, Vector3Int offsetCoord)
    {
        farmRangeMap.gameObject.SetActive(false);
    }
}