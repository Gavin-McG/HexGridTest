using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonUI : MonoBehaviour
{
    [SerializeField] PartyManager pm;

    [Space(10)]

    [SerializeField] GameObject[] fighterPanels;

    [Space(10)]

    [SerializeField] float updateRate = 0.5f;

    [HideInInspector] public Dungeon dungeon;
    FighterPanel[] panelInfo;
    float lastUpdate = 0;


    private void Start()
    {
        UIManager.closeAllUI.AddListener(CloseUI);
    }

    private void OnEnable()
    {
        //check UI sizes
        Debug.Assert(fighterPanels.Length == 4);

        panelInfo = new FighterPanel[4];
        for (int i = 0; i < 4; i++)
        {
            //get adventurer panels
            panelInfo[i] = fighterPanels[i].GetComponent<FighterPanel>();
        }

        //initial UI state
        UpdateUI();
    }

    private void Update()
    {
        if (Time.time >= lastUpdate + updateRate)
        {
            UpdateUI();
        }
    }

    public void UpdateUI()
    {

    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
