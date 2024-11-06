using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject tavernUI;
    [SerializeField] GameObject dungeonUI;

    //tell all UI to close
    public static UnityEvent closeAllUI = new UnityEvent();

    //events
    public static UnityEvent UIOpened = new UnityEvent();
    public static UnityEvent UIClosed = new UnityEvent();

    private void OnEnable()
    {
        BuildingManager.BuildingClicked.AddListener(ClickBuilding);
    }

    private void OnDisable()
    {
        BuildingManager.BuildingClicked.RemoveListener(ClickBuilding);
    }

    void ClickBuilding(Building building, Vector3Int offsetCoords)
    {
        //event calls
        closeAllUI.Invoke();

        //open correct UI
        switch (building.type) 
        {
            case BuildingType.Tavern:
                tavernUI.SetActive(true);
                break;
            case BuildingType.Dungeon:
                if (building is Dungeon dungeon)
                {
                    dungeonUI.GetComponent<DungeonUI>().dungeon = dungeon;
                    dungeonUI.SetActive(true);
                }
                else
                {
                    Debug.LogError("Dungeon '" + building.buildingName + "' BuildingType does not derive from Dungeon Script");
                }
                
                break;
            default:
                break;
        }
    }
}
