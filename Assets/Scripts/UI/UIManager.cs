using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] PartyManager pm;

    private Dictionary<String, GameObject> uiDictionary;

    //tell all UI to close
    public static UnityEvent closeAllUI = new UnityEvent();

    //events
    public static UnityEvent UIOpened = new UnityEvent();
    public static UnityEvent UIClosed = new UnityEvent();
    public static UnityEvent UIAction = new UnityEvent();

    private InputAction closeAction;

    private void Awake()
    {
        closeAllUI.AddListener(OnCloseAllUI);
        
        closeAction = GetComponent<PlayerInput>().actions["CloseMenus"];
        closeAction.performed += _ => closeAllUI.Invoke();

        uiDictionary = UIDictionaryManager.uiDictionary;
    }

    void OnCloseAllUI()
    {
        foreach (GameObject uiObject in uiDictionary.Values)
        {
            uiObject.SetActive(false);
        }
    }

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
                GameObject tavernUI;
                if (uiDictionary.TryGetValue("Tavern", out tavernUI))
                {
                    tavernUI.SetActive(true);
                }
                else
                {
                    Debug.LogError("Tavern UI could not be found!");
                }
                break;
            case BuildingType.Dungeon:
                if (building is Dungeon dungeon)
                {
                    if (pm.fighting && dungeon == pm.dungeon)
                    {
                        //open dungeon UI directly
                        GameObject dungeonUI;
                        if (uiDictionary.TryGetValue("Dungeon", out dungeonUI))
                        {
                            dungeonUI.GetComponent<DungeonUI>().dungeon = dungeon;
                            dungeonUI.SetActive(true);
                        }
                        else
                        {
                            Debug.LogError("Dungeon UI could not be found!");
                        }
                    }
                    else
                    {
                        //open levels UI
                        GameObject dungeonLevelsUI;
                        if (uiDictionary.TryGetValue("DungeonLevels", out dungeonLevelsUI))
                        {
                            dungeonLevelsUI.GetComponent<DungeonLevelsUI>().dungeon = dungeon;
                            dungeonLevelsUI.SetActive(true);
                        }
                        else
                        {
                            Debug.LogError("Dungeon Levels UI could not be found!");
                        }
                    }
                }
                else
                {
                    Debug.LogError("Dungeon '" + building.buildingName + "' BuildingType does not derive from Dungeon Script");
                }
                break;
            case BuildingType.Contractor:
                GameObject purchaseUI;
                if (uiDictionary.TryGetValue("Contractor", out purchaseUI))
                {
                    purchaseUI.SetActive(true);
                }
                else
                {
                    Debug.LogError("Tavern UI could not be found!");
                }
                break;
            default:
                break;
        }
    }
}
