using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FighterPanel : MonoBehaviour
{
    [SerializeField] Image headImage;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image classImage;
    [SerializeField] TextMeshProUGUI classText;
    [SerializeField] RectTransform skillBackground;
    [SerializeField] RectTransform skillBar;
    [SerializeField] RectTransform strengthBar;
    [SerializeField] RectTransform teamworkBar;
    [SerializeField] RectTransform healthBackground;
    [SerializeField] RectTransform healthBar;
    [SerializeField] GameObject deadPanel;
    [SerializeField] GameObject readyPanel;
    [SerializeField] GameObject notReadyPanel;

    public void SetSkills(Skills skill)
    {
        SetSkill(skillBar, skill.skill);
        SetSkill(strengthBar, skill.strength);
        SetSkill(teamworkBar, skill.teamwork);
    }

    void SetSkill(RectTransform bar, float stat)
    {
        bar.sizeDelta = new Vector2(skillBackground.sizeDelta.x, stat * skillBackground.sizeDelta.y);
    }

    public void SetHealth(float health)
    {
        healthBar.sizeDelta = new Vector2(healthBackground.sizeDelta.x, health * healthBackground.sizeDelta.y);
    }

    public void SetHead(Sprite sprite)
    {
        headImage.sprite = sprite;
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void SetClass(string className, Color classColor)
    {
        classImage.color = classColor;
        classText.text = className;
    }

    public void SetDead()
    {
        deadPanel.SetActive(true);
        readyPanel.SetActive(false);
        notReadyPanel.SetActive(false);
    }

    public void SetReady()
    {
        deadPanel.SetActive(false);
        readyPanel.SetActive(true);
        notReadyPanel.SetActive(false);
    }

    public void SetNotReady()
    {
        deadPanel.SetActive(false);
        readyPanel.SetActive(false);
        notReadyPanel.SetActive(true);
    }

    public void clearPanel()
    {
        deadPanel.SetActive(false);
        readyPanel.SetActive(false);
        notReadyPanel.SetActive(false);
    }
}
