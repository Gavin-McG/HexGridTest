using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonUI : MonoBehaviour
{
    [SerializeField] PartyManager pm;

    [Space(10)]

    [SerializeField] GameObject[] fighterPanels;
    [SerializeField] GameObject startButton;

    [Space(10)]

    [SerializeField] float updateRate = 0.5f;

    [HideInInspector] public Dungeon dungeon;
    FighterPanel[] panelInfo;
    float lastUpdate = 0;

    private void OnEnable()
    {
        UIManager.closeAllUI.AddListener(CloseUI);

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

    private void OnDisable()
    {
        UIManager.closeAllUI.RemoveListener(CloseUI);
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
        //update time
        lastUpdate = Time.time;

        Debug.Assert(panelInfo.Length == 4);

        bool canStart = !pm.fighting;

        int adventurerCount = 0;
        for (int i=0; i<4; ++i)
        {
            Adventurer adventurer = pm.GetAdventurer(i);
            if (adventurer != null)
            {
                fighterPanels[i].SetActive(true);

                panelInfo[i].SetHead(adventurer.info.headSprite);
                panelInfo[i].SetSkills(adventurer.skills);
                panelInfo[i].SetName(adventurer.name);
                panelInfo[i].SetHealth(Mathf.Clamp01(adventurer.health/100f));

                //dungeon state indicators
                if (adventurer.state == AdventurerState.Dead)
                {
                    //adventurer is dead
                    panelInfo[i].SetDead();

                }
                else if (pm.dungeon != dungeon)
                {
                    //not ready if at another dungeon
                    panelInfo[i].SetNotReady();
                    canStart = false;
                }
                else if (adventurer.state == AdventurerState.Fighting)
                {
                    //no ready indicator if fighting
                    panelInfo[i].clearPanel();
                }
                else if (adventurer.state == AdventurerState.Ready)
                {
                    panelInfo[i].SetReady();
                }
                else
                {
                    panelInfo[i].SetNotReady();
                    canStart = false;
                }

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

                adventurerCount++;

            }
            else
            {
                fighterPanels[i].SetActive(false);
            }
        }

        //startButton
        canStart = canStart && adventurerCount > 0;
        startButton.SetActive(canStart);
    }

    public void StartFight()
    {
        pm.StartFight(dungeon.difficulty);
        UpdateUI();
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
