using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PartyManager : MonoBehaviour
{
    public BuildingManager bm;

    public GameObject adventurerPrefab;

    public Adventurer[] adventurers = new Adventurer[4];


    private void Start()
    {
        BoundsInt bounds = bm.groundMap.cellBounds;

        HexPoint cell1 = new HexPoint(HexUtils.OffsetToCubic(new Vector3Int(Random.Range(bounds.xMin, bounds.xMax), Random.Range(bounds.yMin, bounds.yMax), 0)), Random.value > 0.5f);
        HexPoint cell2 = new HexPoint(HexUtils.OffsetToCubic(new Vector3Int(Random.Range(bounds.xMin, bounds.xMax), Random.Range(bounds.yMin, bounds.yMax), 0)), Random.value > 0.5f);

        List <Vector3> points = HexAStar.FindPath(cell1, cell2, bm);

        for (int i=0; i<points.Count; i++)
        {
            points[i] -= Vector3.up * 0.3f;
        }

        if (points.Count == 0)
        {
            Debug.Log("Impossible paths");
            return;
        }

        GameObject newAdventerur = Instantiate(adventurerPrefab);
        WalkingAdventurer walker = newAdventerur.GetComponent<WalkingAdventurer>();
        walker.StartPath(adventurers[0], points, 1f);
    }
}
