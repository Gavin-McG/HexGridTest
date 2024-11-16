using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI difficultyText;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI fossilText;
    [SerializeField] GameObject enterButton;


    public void SetDifficulty(int difficulty)
    {
        difficultyText.text = "Difficulty: " + difficulty.ToString();
    }

    public void SetGold(int goldLow, int goldHigh)
    {
        goldText.text = goldLow.ToString() + "-" + goldHigh.ToString();
    }

    public void SetFossil(int fossilCurrent, int fossilLimit)
    {
        fossilText.text = fossilCurrent.ToString() + " / " + fossilLimit.ToString();
    }

    public void SetEnterButtonActive(bool active)
    {
        enterButton.SetActive(active);
    }
}
