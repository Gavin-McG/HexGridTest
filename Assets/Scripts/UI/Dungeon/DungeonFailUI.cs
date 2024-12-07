using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonFailUI : MonoBehaviour
{
    private void OnEnable()
    {
        UIManager.closeAllUI.AddListener(CloseUI);
    }

    private void OnDisable()
    {
        UIManager.closeAllUI.RemoveListener(CloseUI);
    }

    public void CloseUI()
    {
        UIManager.UIAction.Invoke();
        gameObject.SetActive(false);
    }
}
