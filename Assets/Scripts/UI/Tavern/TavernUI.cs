using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;




public class TavernUI : MonoBehaviour
{
    [SerializeField] PartyManager pm;
    [SerializeField] BuildingManager bm;
    [SerializeField] HireUI hireUI;

    [Space(10)]

    [SerializeField] GameObject[] adventurerPanels;
    [SerializeField] GameObject[] hirePanels;


    AdventurerPanel[] panelInfo;

    private void Start()
    {
        UIManager.closeAllUI.AddListener(CloseUI);
    }

    private void OnEnable()
    {
        //check UI sizes
        Debug.Assert(adventurerPanels.Length == 4);
        Debug.Assert(hirePanels.Length == 4);

        panelInfo = new AdventurerPanel[4];
        for (int i=0; i<4; i++)
        {
            //get adventurer panels
            panelInfo[i] = adventurerPanels[i].GetComponent<AdventurerPanel>();
        }

        //initial UI state
        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i=0; i<4; ++i)
        {
            Adventurer adventurer = pm.GetAdventurer(i);
            if (adventurer != null)
            {
                //enable correct panel
                adventurerPanels[i].SetActive(true);
                hirePanels[i].SetActive(false);

                //update panel data
                panelInfo[i].SetHead(adventurer.info.headSprite);
                panelInfo[i].SetName(adventurer.name);
                panelInfo[i].SetSkills(adventurer.skills);
            }
            else
            {
                //enable correct panel
                adventurerPanels[i].SetActive(false);
                hirePanels[i].SetActive(true);
            }
        }
    }


    public void startDispatch(string dungeonName)
    {
        List<Building> buildings = bm.GetBuildingsOfType(BuildingType.Dungeon);
        foreach (Building building in buildings)
        {
            if (building.buildingName == dungeonName && building is Dungeon dungeon)
            {
                Debug.Log("dispatching party");
                //dispatch if correct dungeon found
                pm.DispatchParty(dungeon);
                break;
            }
        }

        //close UI
        CloseUI();
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }

    public void OpenHireUI(int slotNum)
    {
        CloseUI();
        hireUI.gameObject.SetActive(true);
        hireUI.currentHireSlot = slotNum;
    }
}
