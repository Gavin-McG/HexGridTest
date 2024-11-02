using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Building
{
    private float multiplier = 1.5f;
    private float range = 1.5f;

    void Start()
    {
        UpgradeEvent.AddListener(OnUpgrade);
    }

    private Dictionary<int, float> upgradeData = new()
    {
        {1, 1.5f},
        {2, 2f},
        {3, 3f},
        {4, 4f},
        {5, 5f}
    };
    
    public override BuildingType type => BuildingType.Farm;
    
    private void OnUpgrade(Upgrade upgrade, int newLevel)
    {
        if (upgradeData.TryGetValue(newLevel, out var upgradeValues))
        {
            multiplier = upgradeValues;
            Debug.Log($"Farm upgraded to level {level}: multiplier = {multiplier}");
        }
        else
        {
            Debug.LogError($"Invalid upgrade level {newLevel} for Farm.");
        }
    }
}
