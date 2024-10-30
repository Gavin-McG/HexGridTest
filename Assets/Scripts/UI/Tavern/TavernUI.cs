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

    [Space(10)]

    [SerializeField] Color waitingColor = Color.yellow;
    [SerializeField] Color travellingColor = Color.blue;
    [SerializeField] Color readyColor = Color.green;
    [SerializeField] Color returningColor = Color.blue;
    [SerializeField] Color fightingColor = Color.red;
    [SerializeField] Color deadColor = Color.black;

    [Space(10)]

    [SerializeField] Color warriorColor = Color.red;
    [SerializeField] Color archerColor = Color.green;
    [SerializeField] Color mageColor = Color.magenta;

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

                //update class image
                switch (adventurer.info.classType)
                {
                    case ClassType.Warrior:
                        panelInfo[i].SetClass("Warrior", warriorColor);
                        break;
                    case ClassType.Archer:
                        panelInfo[i].SetClass("Archer", archerColor);
                        break;
                    case ClassType.Mage:
                        panelInfo[i].SetClass("Mage", mageColor);
                        break;
                }

                //update state image
                switch (adventurer.state)
                {
                    case AdventurerState.Waiting:
                        panelInfo[i].SetState("Waiting", waitingColor);
                        break;
                    case AdventurerState.Travelling:
                        panelInfo[i].SetState("Travelling", travellingColor);
                        break;
                    case AdventurerState.Ready:
                        panelInfo[i].SetState("Ready", readyColor);
                        break;
                    case AdventurerState.Returning:
                        panelInfo[i].SetState("Returning", returningColor);
                        break;
                    case AdventurerState.Fighting:
                        panelInfo[i].SetState("Fighting", fightingColor);
                        break;
                    case AdventurerState.Dead:
                        panelInfo[i].SetState("Dead", deadColor);
                        break;
                }
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
