using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdventurerPanel : MonoBehaviour
{
    [SerializeField] Image headImage;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image classImage;
    [SerializeField] TextMeshProUGUI classText;
    [SerializeField] Image stateImage;
    [SerializeField] TextMeshProUGUI stateText;
    [SerializeField] RectTransform SkillBar;
    [SerializeField] RectTransform strengthBar;
    [SerializeField] RectTransform teamworkBar;

    public void SetSkills(Skills skill)
    {
        SetSkill(SkillBar, skill.skill);
        SetSkill(strengthBar, skill.strength);
        SetSkill(teamworkBar, skill.teamwork);
    }

    void SetSkill(RectTransform bar, float stat)
    {
        bar.sizeDelta = new Vector2(stat*250, 5);
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

    public void SetState(string stateName, Color stateColor)
    {
        stateImage.color = stateColor;
        stateText.text = stateName;
    }
}
