using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HireUI : MonoBehaviour
{
    [SerializeField] PartyManager pm;
    [SerializeField] ResourceManager rm;
    [SerializeField] TavernUI tavernUI;

    [Space(10)]

    [SerializeField] AdventurerHirePanel[] hirePanels;
    [SerializeField] Resources hireCost;


    [HideInInspector] public int currentHireSlot = 0;
    Adventurer[] adventurers;


    private void Awake()
    {
        adventurers = new Adventurer[hirePanels.Length];
        UpdateSelections();
    }


    private void OnEnable()
    {
        UIManager.closeAllUI.AddListener(CloseUI);

        UpdateUI();
    }

    private void OnDisable()
    {
        UIManager.closeAllUI.RemoveListener(CloseUI);
    }

    private void UpdateUI()
    {
        for (int i = 0; i < adventurers.Length; ++i)
        {
            //update panel data
            hirePanels[i].SetHead(adventurers[i].info.headSprite);
            hirePanels[i].SetName(adventurers[i].name);
            hirePanels[i].SetSkills(adventurers[i].skills);

            //update class image
            switch (adventurers[i].info.classType)
            {
                case ClassType.Warrior:
                    hirePanels[i].SetClass("Warrior", pm.warriorColor);
                    break;
                case ClassType.Archer:
                    hirePanels[i].SetClass("Archer", pm.archerColor);
                    break;
                case ClassType.Mage:
                    hirePanels[i].SetClass("Mage", pm.mageColor);
                    break;
            }
        }
    }


    private void UpdateSelections()
    {
        for (int i = 0; i<adventurers.Length; ++i)
        {
            adventurers[i] = pm.GenerateAdevnturer();
        }
    }


    public void HireAdventurer(int adventurerNum)
    {
        //check cost
        if (!rm.CanAfford(hireCost)) return;
        rm.Charge(hireCost);

        //hire adventurer
        pm.HireAdventurer(currentHireSlot,adventurers[adventurerNum]);

        //reset with new selection
        UpdateSelections();

        //go back to tavernUI
        OpenTavernUI();
    }


    public void CloseUI()
    {
        gameObject.SetActive(false);
    }

    public void OpenTavernUI()
    {
        CloseUI();
        tavernUI.gameObject.SetActive(true);
    }
}
