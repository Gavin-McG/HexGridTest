using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class BuildingMenu : MonoBehaviour
{
    private Building curBuilding;
    
    [SerializeField] private TextMeshProUGUI curBuildingText;

    [SerializeField] private TextMeshProUGUI curBuildingDescription;
    
    [SerializeField] private TextMeshProUGUI curBuildingCostMagic;
    [SerializeField] private TextMeshProUGUI curBuildingCostWood;
    [SerializeField] private TextMeshProUGUI curBuildingCostStone;
    
    //This needs some more looking at since the structures are created dynamically right now
    //[SerializeField] private Image curBuildingImage;

    [SerializeField] private Button buildButton;

    private void Awake()
    {
        buildButton.onClick.AddListener(OnBuildButtonClick);
    }

    private void OnEnable()
    {
        UIManager.UIOpened.Invoke();
    }

    private void OnDisable()
    {
        UIManager.UIClosed.Invoke();
    }

    public void OnClick(Building building)
    {
        curBuilding = building;
        
        curBuildingText.text = building.buildingName;

        curBuildingDescription.text = building.descriptionText;
        
        curBuildingCostMagic.text = building.buildCost.Magic.ToString();
        curBuildingCostWood.text = building.buildCost.Wood.ToString();
        curBuildingCostStone.text = building.buildCost.Stone.ToString();
        
        curBuildingText.transform.parent.gameObject.SetActive(true);
    }

    public void OnBuildButtonClick()
    {
        if (curBuilding != null)
        {
            BuildingManager.EnableBuilding.Invoke(curBuilding.currentStructure);
            gameObject.SetActive(false);
            curBuildingText.transform.parent.gameObject.SetActive(false);
        }
    }

    public void DebugMessage()
    {
        Debug.LogError("This building has not be implemented yet!");
    }
}
