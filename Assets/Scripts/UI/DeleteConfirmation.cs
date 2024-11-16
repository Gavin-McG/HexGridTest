using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeleteConfirmation : MonoBehaviour
{
    private BuildingManager bm;
    
    [SerializeField] private TextMeshProUGUI irreversableText;
    [SerializeField] private TextMeshProUGUI wizardWarningText;

    [Header("Refund Amount")] 
    [SerializeField] private TextMeshProUGUI magicRefundText;
    [SerializeField] private TextMeshProUGUI woodRefundText;
    [SerializeField] private TextMeshProUGUI stoneRefundText;
    
    private Building currentBuilding;
    private Vector3Int curBuildingOffset;

    void Start()
    {
        bm = FindObjectOfType<BuildingManager>();
    }

    public void SetDeleteUI(Building building, Vector3Int buildingOffset)
    {
        currentBuilding = building;
        curBuildingOffset = buildingOffset;
        
        if (currentBuilding.type == BuildingType.WizardTower)
        {
            wizardWarningText.gameObject.SetActive(true);
        }
        else
        {
            wizardWarningText.gameObject.SetActive(false);
        }

        string formattedName = building.name.Replace("(Clone)", "").Trim();
        irreversableText.text = "Delete selected " + formattedName + "?";
        
        Resources refundAmount = currentBuilding.buildCost * ResourceManager.Instance.refundRate;
        magicRefundText.text = refundAmount.Magic.ToString();
        woodRefundText.text = refundAmount.Wood.ToString();
        stoneRefundText.text = refundAmount.Stone.ToString();
        
        gameObject.SetActive(true);
    }

    public void ConfirmButtonClick()
    {
        gameObject.SetActive(false);
        bm.DeleteBuilding(curBuildingOffset);
    }

    public void CancelButtonClick()
    {
        UIManager.closeAllUI.Invoke();
        BuildingManager.FailedDestroy.Invoke();
    }
}
