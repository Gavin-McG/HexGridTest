using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HexPointTest : MonoBehaviour
{
    [SerializeField] GameObject startObj;
    [SerializeField] GameObject goalObj;
    [SerializeField] Tilemap groundMap;
    [SerializeField] BuildingManager bm;
    [SerializeField] GameObject indicator;

    List<GameObject> list;

    bool changed = true;

    float lastTime = 0;

    private void OnValidate()
    {
        changed = true;
    }

    private void Update()
    {
        if (changed || Time.time-lastTime>1f)
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

            HexPoint start = new HexPoint(HexUtils.OffsetToCubic(groundMap.WorldToCell(startObj.transform.position)), true);
            HexPoint goal = new HexPoint(HexUtils.OffsetToCubic(groundMap.WorldToCell(goalObj.transform.position)), true);
            List<Vector3> points = HexAStar.FindPath(start, goal, bm);

            if (points.Count == 0)
            {
                print("No path");
            }

            foreach (Vector3 point in points)
            {
                list.Add(Instantiate(indicator, point, Quaternion.identity));
            }

            changed = false;
            lastTime = Time.time;
        }
    }
}
