using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdventurerPanelMini : MonoBehaviour
{
    [SerializeField] Image headImage;
    [SerializeField] Image stateImage;
    [SerializeField] RectTransform healthBar;
    [SerializeField] RectTransform healthBarBackground;

    public void SetHealth(float health)
    {
        healthBar.sizeDelta = new Vector2(health*healthBarBackground.sizeDelta.x, healthBarBackground.sizeDelta.y);
    }

    public void SetHead(Sprite sprite)
    {
        headImage.sprite = sprite;
    }

    public void SetState(Color stateColor)
    {
        stateImage.color = stateColor;
    }
}
