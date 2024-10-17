using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] ResourceManager rm;

    [Space(10)]

    [SerializeField] TextMeshProUGUI MagicText;
    [SerializeField] TextMeshProUGUI WoodText;
    [SerializeField] TextMeshProUGUI StoneText;

    // Update is called once per frame
    void Update()
    {
        MagicText.text = rm.currentResource.Magic.ToString();
        WoodText.text = rm.currentResource.Wood.ToString();
        StoneText.text = rm.currentResource.Stone.ToString();
    }
}
