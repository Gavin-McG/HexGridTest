using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HexHighlight : MonoBehaviour
{
    public Grid grid;
    public Tilemap tilemap;

    private Vector3Int lastTilePosition;

    // Update is called once per frame
    void Update()
    {
        HighlightTileUnderMouse();
    }

    void HighlightTileUnderMouse()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0;

        Vector3Int cellPosition = grid.WorldToCell(worldPosition);

        if (cellPosition != lastTilePosition)
        {
            if(tilemap.HasTile(lastTilePosition))
            {
                tilemap.SetColor(lastTilePosition, Color.white);
            }
            
            if (tilemap.HasTile(cellPosition))
            {
                tilemap.SetTileFlags(cellPosition, TileFlags.None);
                tilemap.SetColor(cellPosition, Color.red);
            }

            lastTilePosition = cellPosition;
        }
    }
}
