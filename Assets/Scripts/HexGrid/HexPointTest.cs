using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HexPointTest : MonoBehaviour
{
    [SerializeField] Vector3Int goaloffset;
    [SerializeField] Tilemap groundMap;
    [SerializeField] BuildingManager bm;
    [SerializeField] GameObject indicator;

    List<GameObject> list;

    // Update is called once per frame
    void OnEnable()
    {
        if (list != null)
        {
            foreach (GameObject item in list)
            {
                Destroy(item);
            }
            list.Clear();
        }
        else
        {
            list = new List<GameObject>();
        }

        HexPoint start = new HexPoint(new Vector3Int(0, 0, 0), true);
        HexPoint goal = new HexPoint(HexUtils.OffsetToCubic(goaloffset), true);
        List<HexPoint> points = HexAStar.FindPath(start, goal, groundMap, bm);

        foreach (HexPoint point in points)
        {
            Vector3 pos = groundMap.CellToLocal(HexUtils.CubicToOffset(point.cubicCoord));
            pos += Vector3.up * ((point.isTop ? +1 : -1) * groundMap.cellSize.y/2 + 0.11f);
            list.Add(Instantiate(indicator, pos, Quaternion.identity));
        }
    }
}
