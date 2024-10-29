using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HireUI : MonoBehaviour
{
    [SerializeField] PartyManager pm;
    [SerializeField] TavernUI tavernUI;

    [Space(10)]

    [SerializeField] AdventurerHirePanel[] hirePanels;


    [HideInInspector] public int currentHireSlot = 0;
    Adventurer[] adventurers;


    private void Start()
    {
        UIManager.closeAllUI.AddListener(CloseUI);
        adventurers = new Adventurer[hirePanels.Length];
        UpdateSelections();
    }


    private void OnEnable()
    {
        UpdateUI();
    }


    private void UpdateUI()
    {
        for (int i = 0; i < pm.adventurers.Length; ++i)
        {
            //update panel data
            hirePanels[i].SetHead(pm.adventurers[i].info.headSprite);
            hirePanels[i].SetName(pm.adventurers[i].name);
            hirePanels[i].SetSkills(pm.adventurers[i].skills);
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
        //check adventurer slot
        if (pm.adventurers[currentHireSlot] != null)
        {
            Debug.Log("Attempting to hire adventurer in aalready filled slot");
            return;
        }

        //hire adventurer
        pm.adventurers[currentHireSlot] = adventurers[adventurerNum];

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
