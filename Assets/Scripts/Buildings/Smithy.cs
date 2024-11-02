using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smithy : Building
{
    private int armLevel = 1;
    private int goldProduction = 0;

    private void Start()
    {
        UpgradeEvent.AddListener(OnUpgrade);
        rm.RegisterBuilding(this, new Resources(0, 0, 0, goldProduction));
    }

    public override BuildingType type => BuildingType.Smithy;

    private Dictionary<int, (int armLevel, int goldProduction)> upgradeData = new()
    {
        { 1, (1, 0) },
        { 2, (2, 0) },
        { 3, (3, 1) },
        { 4, (4, 2) },
        { 5, (4, 4) }
    }; 
    
    private void OnUpgrade(Upgrade upgrade, int newLevel)
    {
        if (upgradeData.TryGetValue(newLevel, out var upgradeValues))
        {
            armLevel = upgradeValues.armLevel;
            goldProduction = upgradeValues.goldProduction;
            Debug.Log($"Smithy upgraded to level {level}: armLevel = {armLevel}, goldProduction = {goldProduction}");
        }
        else
        {
            Debug.LogError($"Invalid upgrade level {newLevel} for Smithy");
        }
    }
}
