using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class ModeEdit : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    
    [SerializeField] private Structure defaultBuilding;
    [SerializeField] private GameObject typeList;
    [SerializeField] private PlayerInput playerInput;

    private InputAction noneMode;
    private InputAction buildMode;
    private InputAction deleteMode;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        
        noneMode = playerInput.actions["NoneMode"];
        buildMode = playerInput.actions["BuildMode"];
        deleteMode = playerInput.actions["DeleteMode"];

        noneMode.performed += _ => ChangeMode(0);
        buildMode.performed += _ => ChangeMode(1);
        deleteMode.performed += _ => ChangeMode(2);
    }

    public void ChangeMode(int modeIndex)
    {
        dropdown.value = modeIndex;
        UIManager.UIAction.Invoke();
        switch (modeIndex)
        {
            case 0:
                BuildingManager.DisableEditing.Invoke();
                typeList.SetActive(false);
                break;
            case 1:
                typeList.SetActive(true);
                break;
            case 2:
                BuildingManager.EnableDeleting.Invoke();
                typeList.SetActive(false);
                break;
            default:
                Debug.LogError("Mode index at index " + modeIndex + " is out of range!");
                break;
        }
    }
}
