using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class RangeManager : MonoBehaviour
{
    [SerializeField] Tilemap rangeMap;
    [SerializeField] TileBase highlightTile;

    private void OnEnable()
    {
        BuildingManager.BuildingPlaced.AddListener(PlaceRange);
    }

    private void OnDisable()
    {
        BuildingManager.BuildingPlaced.RemoveListener(PlaceRange);
    }


    void PlaceRange(Building building, Vector3Int offsetCoord)
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

        //place tiles
        for (int i=0; i<=range; i++)
        {
            PlaceRing(i, HexUtils.OffsetToCubic(offsetCoord));
        }
    }

    void PlaceRing(int radius, Vector3Int centerCubic)
    {
        if (radius < 0) return;

        //set single tile for radius=0
        if (radius == 0)
        {
            rangeMap.SetTile(HexUtils.CubicToOffset(centerCubic), highlightTile);
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
                rangeMap.SetTile(offsetPosition, highlightTile);

                // Move to the next position in the current direction
                currentCubic += directions[i];
            }
        }
    }
}
