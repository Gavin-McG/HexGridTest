using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PartyManager : MonoBehaviour
{
    public BuildingManager bm;
    public GameObject adventurerPrefab;

    [Space(10)]

    [SerializeField] float dispatchDelay = 0.2f;
    [SerializeField] float walkSpeed = 1f;

    public Adventurer[] adventurers = new Adventurer[4];

    private Tavern GetTavern()
    {
        //get tavern(s)
        List<Building> taverns = bm.GetBuildingsOfType(BuildingType.Tavern);

        //check tavern count
        if (taverns.Count == 0)
        {
            Debug.LogError("Attempting to dispatch party without a tavern");
            return null;
        }
        if (taverns.Count > 1)
        {
            Debug.Log("Attempting to dispatch a party with multiple taverns");
            return null;
        }

        //check tavern type
        if (taverns[0] is Tavern tavern)
        {
            return tavern;
        }

        //wrong type
        Debug.LogError("Tavern building is not of 'Tavern' ckass");
        return null;
    }

    public void DispatchParty(Dungeon dungeon)
    {
        Tavern tavern = GetTavern();

        //check buildings
        if (tavern == null || dungeon == null) return;

        //get path
        List<Vector3> path = HexAStar.FindPath(tavern.exit, dungeon.entrance, bm);
        Debug.Log(tavern.exit);
        Debug.Log(dungeon.entrance);
        if (path.Count == 0)
        {
            Debug.LogWarning("Could not find valid path between Tavern and selcted Dungeon");
            return;
        }

        //start dispatch
        StartCoroutine(DispatchRoutine(path));
    }

    IEnumerator DispatchRoutine(List<Vector3> path)
    {
        for (int i=0; i<adventurers.Length; ++i)
        {
            //wait for next adventurer
            yield return new WaitForSeconds(dispatchDelay);

            //create adventurer
            Debug.Log("Spawn Adventurer");
            GameObject newAdventurer = Instantiate(adventurerPrefab, path[0], Quaternion.identity);
            WalkingAdventurer walker = newAdventurer.GetComponent<WalkingAdventurer>();
            walker.StartPath(adventurers[i], path, walkSpeed);

        }
    }


    bool hasDispatched = false;
    private void Update()
    {
        if (!hasDispatched && bm.GetBuildingsOfType(BuildingType.Tavern).Count > 0)
        {
            Building dungeonBuilding = bm.GetBuildingsOfType(BuildingType.Dungeon)[0];
            if (dungeonBuilding == null) return;
            if (dungeonBuilding is Dungeon dungeon)
            {
                DispatchParty(dungeon);
            }
            hasDispatched = true;
        }
    }
}
