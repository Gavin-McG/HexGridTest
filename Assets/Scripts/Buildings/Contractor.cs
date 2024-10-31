using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ContractorUpgradeData
{
    public int stoneProduction;
    public List<float> buildingLevelCosts;
}

public class Contractor : Building
{
    private int stoneProduction = 1;
    private List<float> buildingLevelCosts = new List<float>{ -1f, -1f, -1f, -1f, -1f };

    void Awake()
    {
        UpgradeEvent.AddListener(OnUpgrade);
    }

    private Dictionary<int, (int stoneProduction, List<float> buildingLevelCosts)> upgradeData = new()
    {
        {1, (1, new List<float>{1.0f, -1.0f, -1.0f, -1.0f, -1.0f}) },
        {2, (2, new List<float>{0.8f, 1.0f, -1.0f, -1.0f, -1.0f}) },
        {3, (4, new List<float>{0.6f, 0.8f, 1.0f, -1.0f, -1.0f}) },
        {4, (8, new List<float>{0.6f, 0.6f, 0.8f, 1.0f, -1.0f}) },
        {5, (16, new List<float>{0.6f, 0.6f, 0.6f, 0.8f, 1.0f}) },
    };

    public override BuildingType type => BuildingType.Contractor;

    private void OnUpgrade(Upgrade upgrade, int newLevel)
    {
        if (upgradeData.TryGetValue(newLevel, out var upgradeValues))
        {
            stoneProduction = upgradeValues.stoneProduction;
            buildingLevelCosts = upgradeValues.buildingLevelCosts;
            currentStructure = upgrade.newStructure;
            level = newLevel;
            Debug.Log($"Contractor upgraded to level {level}: stoneProduction = {stoneProduction}");
        }
        else
        {
            Debug.LogError($"Invalid upgrade level {newLevel} for Contractor.");
        }
    }
}

