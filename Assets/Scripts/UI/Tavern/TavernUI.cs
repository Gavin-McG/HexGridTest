using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;




public class TavernUI : MonoBehaviour
{
    [SerializeField] PartyManager pm;
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
        Debug.Assert(adventurerPanels.Length == pm.adventurers.Length);
        Debug.Assert(hirePanels.Length == pm.adventurers.Length);

        panelInfo = new AdventurerPanel[adventurerPanels.Length];
        for (int i=0; i<pm.adventurers.Length; i++)
        {
            //get adventurer panels
            panelInfo[i] = adventurerPanels[i].GetComponent<AdventurerPanel>();
        }

        //initial UI state
        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i=0; i<pm.adventurers.Length; ++i)
        {
            if (pm.adventurers[i] != null)
            {
                //enable correct panel
                adventurerPanels[i].SetActive(false);
                hirePanels[i].SetActive(true);

                //update panel data
                panelInfo[i].SetHead(pm.adventurers[i].info.headSprite);
                panelInfo[i].SetName(pm.adventurers[i].name);
                panelInfo[i].SetSkills(pm.adventurers[i].skills);
            }
            else
            {
                //enable correct panel
                adventurerPanels[i].SetActive(false);
                hirePanels[i].SetActive(true);
            }
        }
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
