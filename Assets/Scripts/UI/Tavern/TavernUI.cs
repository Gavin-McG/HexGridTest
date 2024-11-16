using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    [Space(10)]

    [SerializeField] float updateRate = 0.5f;

    AdventurerPanel[] panelInfo;
    float lastUpdate = 0;


    private void OnEnable()
    {
        UIManager.closeAllUI.AddListener(CloseUI);
        UIManager.UIOpened.Invoke();

        //check UI sizes
        Debug.Assert(adventurerPanels.Length == pm.adventurers.Length);
        Debug.Assert(hirePanels.Length == pm.adventurers.Length);

        panelInfo = new AdventurerPanel[pm.adventurers.Length];
        for (int i=0; i<pm.adventurers.Length; i++)
        {
            //get adventurer panels
            panelInfo[i] = adventurerPanels[i].GetComponent<AdventurerPanel>();
        }

        //initial UI state
        UpdateUI();
    }

    private void OnDisable()
    {
        UIManager.closeAllUI.RemoveListener(CloseUI);
        UIManager.UIClosed.Invoke();
    }

    private void Update()
    {
        if (Time.time >= lastUpdate + updateRate) 
        {
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        //update time
        lastUpdate = Time.time;

        //update ui elements
        for (int i=0; i<pm.adventurers.Length; ++i)
        {
            Adventurer adventurer = pm.adventurers[i];
            if (adventurer != null)
            {
                //enable correct panel
                adventurerPanels[i].SetActive(true);
                hirePanels[i].SetActive(false);

                //update panel data
                panelInfo[i].SetHead(adventurer.info.headSprite);
                panelInfo[i].SetName(adventurer.name);
                panelInfo[i].SetSkills(adventurer.skills);

                //update class image
                switch (adventurer.info.classType)
                {
                    case ClassType.Warrior:
                        panelInfo[i].SetClass("Warrior", pm.warriorColor);
                        break;
                    case ClassType.Archer:
                        panelInfo[i].SetClass("Archer", pm.archerColor);
                        break;
                    case ClassType.Mage:
                        panelInfo[i].SetClass("Mage", pm.mageColor);
                        break;
                }

                //update state image
                switch (adventurer.state)
                {
                    case AdventurerState.Waiting:
                        panelInfo[i].SetState("Waiting", pm.waitingColor);
                        break;
                    case AdventurerState.Travelling:
                        panelInfo[i].SetState("Travelling", pm.travellingColor);
                        break;
                    case AdventurerState.Ready:
                        panelInfo[i].SetState("Ready", pm.readyColor);
                        break;
                    case AdventurerState.Returning:
                        panelInfo[i].SetState("Returning", pm.returningColor);
                        break;
                    case AdventurerState.Fighting:
                        panelInfo[i].SetState("Fighting", pm.fightingColor);
                        break;
                    case AdventurerState.Dead:
                        panelInfo[i].SetState("Dead", pm.deadColor);
                        break;
                }
            }
            else if ((i == 0 || pm.adventurers[i-1]!=null) && pm.CanHire())
            {
                //enable correct panel
                adventurerPanels[i].SetActive(false);
                hirePanels[i].SetActive(true);
            }
            else
            {
                //enable correct panel
                adventurerPanels[i].SetActive(false);
                hirePanels[i].SetActive(false);
            }
        }
    }


    public void startDispatch(string dungeonName)
    {
        //check validity
        if (!pm.CanDispatch()) return;

        //find correcct dungeon
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
