using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

//Can be used as read parameters for other buildings
public class ContractorUpgradeData
{
    public int stoneProduction;
    public List<float> buildingLevelCosts;
}

/* Author: Connor Spears
 * Date: 10/31/2024
 * Description: A definition for the Contractor building, this inherits from the Building class and implements its methods.
 *              Defines upgrades and building functionality.
*/ 
public class Contractor : Building
{
    private int stoneProduction = 1;
    private List<float> buildingLevelCosts = new List<float>{ -1f, -1f, -1f, -1f, -1f };

    void Start()
    {
        UpgradeEvent.AddListener(OnUpgrade);
        rm.RegisterBuilding(this, new Resources(0,0, stoneProduction));
    }

    /* Holds all the data for each upgrade(-1 means that level is not unlocked yet)
     * Ideally this would be stored somewhere rather than just a dictionary, SOs don't work afaik
    */
    private Dictionary<int, (int stoneProduction, List<float> buildingLevelCosts)> upgradeData = new()
    {
        {1, (1, new List<float>{1.0f, -1.0f, -1.0f, -1.0f, -1.0f}) },
        {2, (2, new List<float>{0.8f, 1.0f, -1.0f, -1.0f, -1.0f}) },
        {3, (4, new List<float>{0.6f, 0.8f, 1.0f, -1.0f, -1.0f}) },
        {4, (8, new List<float>{0.6f, 0.6f, 0.8f, 1.0f, -1.0f}) },
        {5, (16, new List<float>{0.6f, 0.6f, 0.6f, 0.8f, 1.0f}) },
    };

    public override BuildingType type => BuildingType.Contractor;

    //Defines what to do when an upgrade is purchased
    private void OnUpgrade(Upgrade upgrade, int newLevel)
    {
        if (upgradeData.TryGetValue(newLevel, out var upgradeValues))
        {
            stoneProduction = upgradeValues.stoneProduction;
            buildingLevelCosts = upgradeValues.buildingLevelCosts;
            Debug.Log($"Contractor upgraded to level {level}: stoneProduction = {stoneProduction}");
        }
        else
        {
            Debug.LogError($"Invalid upgrade level {newLevel} for Contractor.");
        }
    }
}

