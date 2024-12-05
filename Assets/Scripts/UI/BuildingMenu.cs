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
    
    [SerializeField] private Image curBuildingImage;

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
        UIManager.UIAction.Invoke();

        curBuilding = building;
        
        curBuildingText.text = building.buildingName;

        curBuildingDescription.text = building.descriptionText;
        
        curBuildingCostMagic.text = building.buildCost.Magic.ToString();
        curBuildingCostWood.text = building.buildCost.Wood.ToString();
        curBuildingCostStone.text = building.buildCost.Stone.ToString();

        //Get the selected building's first structure piece and the associated sprite's image
        curBuildingImage.sprite = building.currentStructure.pieces[0].tile.sprite;
        
        curBuildingText.transform.parent.gameObject.SetActive(true);
    }

    public void OnBuildButtonClick()
    {
        UIManager.UIAction.Invoke();

        if (curBuilding != null)
        {
            BuildingManager.EnableBuilding.Invoke(curBuilding.currentStructure);
            gameObject.SetActive(false);
            curBuildingText.transform.parent.gameObject.SetActive(false);
        }
    }
}
