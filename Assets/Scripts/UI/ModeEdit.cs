using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.Serialization;

public class ModeEdit : MonoBehaviour
{
    [SerializeField] private Structure defaultBuilding;
    [SerializeField] private GameObject TypeList;
    
    public void ChangeMode(int modeIndex)
    {
        switch (modeIndex)
        {
            case 0:
                BuildingManager.DisableEditing.Invoke();
                TypeList.SetActive(false);
                break;
            case 1:
                BuildingManager.EnableBuilding.Invoke(defaultBuilding);
                TypeList.SetActive(true);
                break;
            case 2:
                BuildingManager.EnableDeleting.Invoke();
                TypeList.SetActive(false);
                break;
            default:
                Debug.LogError("Mode index at index " + modeIndex + " is out of range!");
                break;
        }
    }
}
