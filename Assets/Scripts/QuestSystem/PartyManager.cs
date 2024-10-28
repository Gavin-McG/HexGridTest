using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PartyManager : MonoBehaviour
{
    [SerializeField] BuildingManager bm;
    [SerializeField] GameObject adventurerPrefab;
    [SerializeField] AdventurerCollection collection;

    [Space(10)]

    [SerializeField] float dispatchDelay = 0.2f;

    public Adventurer[] adventurers = new Adventurer[4];




    private void Update()
    {
        Debug.Log(GenerateAdevnturer().skills.teamwork);
    }



    //get the tavern
    private Tavern GetTavern()
    {
        //get tavern(s)
        List<Building> taverns = bm.GetBuildingsOfType(BuildingType.Tavern);

        //check tavern count
        if (taverns.Count == 0)
        {
            Debug.LogWarning("Attempting to look for tavern without tavern");
            return null;
        }
        if (taverns.Count > 1)
        {
            Debug.Log("Found multiple taverns");
            return null;
        }

        //check tavern type
        if (taverns[0] is Tavern tavern)
        {
            return tavern;
        }

        //wrong type
        Debug.LogError("Tavern building is not of 'Tavern' class");
        return null;
    }




    //dispatch party towards dungeon
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
            Debug.LogWarning("Could not find valid path between Tavern and selected Dungeon");
            return;
        }

        //start dispatch
        StartCoroutine(DispatchRoutine(path));
    }



    //dispatch adventurers 1 by 1
    IEnumerator DispatchRoutine(List<Vector3> path)
    {
        for (int i=0; i<adventurers.Length; ++i)
        {
            //wait for next adventurer
            yield return new WaitForSeconds(dispatchDelay);

            //create walking adventurer character
            Debug.Log("Spawn Adventurer");
            GameObject newAdventurer = Instantiate(adventurerPrefab, path[0], Quaternion.identity);
            WalkingAdventurer walker = newAdventurer.GetComponent<WalkingAdventurer>();
            walker.StartPath(adventurers[i], path);
        }
    }



    Adventurer GenerateAdevnturer()
    {
        Tavern tavern = GetTavern();

        if (tavern==null)
        {
            Debug.LogError("Attempting to generate adventurer without a valid tavern");
            return null;
        }

        return new Adventurer(GetRandomSkills(tavern.averageSkill, 0.2f), GetRandomInfo(), "New Adventurer");
    }

    float GetRandomValue(float mean, float std)
    {
        float value = -1;
        while (value < 0 || value > 1)
        {
            // Generate a standard normal distribution with mean 0 and standard deviation 1
            float u1 = 1.0f - Random.value; // uniform(0,1] random values
            float u2 = 1.0f - Random.value;
            float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);

            // Scale to our desired spread and center around the target
            value = mean + randStdNormal * std;
        }
        return value;
    }

    Skills GetRandomSkills(float mean, float std)
    {
        return new Skills(
            GetRandomValue(mean, std),
            GetRandomValue(std, mean),
            GetRandomValue(std, mean)
        );
    }

    AdventurerInfo GetRandomInfo()
    {
        int index = Random.Range(0, collection.data.Length);
        return collection.data[index];
    }



    public bool FireAdventurer(int index)
    {
        if (adventurers[index]==null)
        {
            Debug.LogError("Attenpting to Fire empty adevnturer slot");
            return false;
        }

        adventurers[index] = null;
        return true;
    }

    public bool HireAdventurer(int index, Adventurer adventurer)
    {
        if (adventurers[index] != null)
        {
            Debug.LogError("Attenpting to Hire in non-empty adevnturer slot");
            return false;
        }

        adventurers[index] = adventurer;
        return true;
    }
}
