using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject tavernUI;

    public static UnityEvent closeAllUI = new UnityEvent();

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
        closeAllUI.Invoke();
        switch (building.type) 
        {
            case BuildingType.Tavern:
                tavernUI.SetActive(true);
                break;
            default:
                break;
        }
    }
}
