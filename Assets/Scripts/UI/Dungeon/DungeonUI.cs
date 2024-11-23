using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonUI : MonoBehaviour
{
    [SerializeField] PartyManager pm;
    [SerializeField] GameObject DungeonMiniUI;

    [Space(10)]

    [SerializeField] GameObject[] fighterPanels;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject returnButton;
    [SerializeField] RectTransform progressBackground;
    [SerializeField] RectTransform progressBar;
    [SerializeField] RectTransform eventPanel;
    [SerializeField] RectTransform[] textPositions;

    [Space(10)]

    [SerializeField] float updateRate = 0.5f;

    [HideInInspector] public Dungeon dungeon;
    [HideInInspector] public int level;
    FighterPanel[] panelInfo;
    float lastUpdate = 0;
    TextPanel[] textPanels;

    private void Awake()
    {
        textPanels = new TextPanel[textPositions.Length];
        for (int i=0; i<textPositions.Length; i++)
        {
            textPanels[i] = textPositions[i].GetComponent<TextPanel>();
        }
    }

    private void OnEnable()
    {
        PartyManager.fightEvent.AddListener(newTextEvent);
        PartyManager.battleFinished.AddListener(ClearText);

        UIManager.closeAllUI.AddListener(CloseUI);
        UIManager.UIOpened.Invoke();

        DungeonMiniUI.SetActive(false);

        //check UI sizes
        Debug.Assert(fighterPanels.Length == pm.adventurers.Length);

        panelInfo = new FighterPanel[pm.adventurers.Length];
        for (int i=0; i<pm.adventurers.Length; i++)
        {
            //get adventurer panels
            panelInfo[i] = fighterPanels[i].GetComponent<FighterPanel>();
        }

        //initial UI state
        ClearText();
        UpdateUI();
    }

    private void OnDisable()
    {
        PartyManager.fightEvent.RemoveListener(newTextEvent);
        PartyManager.battleFinished.RemoveListener(ClearText);

        UIManager.closeAllUI.RemoveListener(CloseUI);
        UIManager.UIClosed.Invoke();

        DungeonMiniUI.SetActive(true);
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

        for (int i=0; i<pm.adventurers.Length; ++i)
        {
            Adventurer adventurer = pm.adventurers[i];
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
            }
            else
            {
                fighterPanels[i].SetActive(false);
            }
        }

        //progress bar
        if (dungeon == pm.dungeon)
        {
            progressBar.sizeDelta = new Vector2(progressBackground.sizeDelta.x * pm.progress, progressBackground.sizeDelta.y);
        }
        else
        {
            progressBar.sizeDelta = Vector2.zero;
        }

        //start Button
        bool canFight = pm.CanFight(dungeon);
        startButton.SetActive(canFight);
        returnButton.SetActive(canFight);
    }

    public void StartFight()
    {
        pm.StartFight(dungeon.levels[level].difficulty);
        pm.level = level;
        UpdateUI();
    }

    public void ReturnAdventurers()
    {
        //check that adventurers can fight
        if (!pm.CanFight(dungeon)) return;

        pm.ReturnParty(dungeon);

        CloseUI();
    }

    void newTextEvent(string text)
    {
        if (pm.dungeon != dungeon) return;

        for (int i=textPanels.Length-1; i>0; i--)
        {
            if (textPanels[i-1].gameObject.activeSelf)
            {
                textPanels[i].gameObject.SetActive(true);
            }
            textPanels[i].text.text = textPanels[i - 1].text.text;
        }
        textPanels[0].text.text = text;
        textPanels[0].gameObject.SetActive(true);
    }

    public void ClearText()
    {
        for (int i=0; i<textPanels.Length; i++)
        {
            textPanels[i].gameObject.SetActive(false);
        }
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
