using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdventurerHirePanel : MonoBehaviour
{
    [SerializeField] Image headImage;
    [SerializeField] TextMeshProUGUI nameText;
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
        bar.sizeDelta = new Vector2(stat * 250, 5);
    }

    public void SetHead(Sprite sprite)
    {
        headImage.sprite = sprite;
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }
}
