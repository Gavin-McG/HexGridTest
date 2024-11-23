using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardTower : Building
{
    [SerializeField] public int buildRange = 4;
    private float radius = 1f;
    private Resources magicProduction = new Resources(1, 0, 0, 0);
    private float productionMultiplier = 1f;
    
    private void Start()
    {
        UpgradeEvent.AddListener(OnUpgrade);
        rm.RegisterBuilding(this);
    }

    public override BuildingType type => BuildingType.WizardTower;

    private Dictionary<int, (float radius, int magicProduction)> upgradeData = new()
    {
        { 1, (1f, 1) },
        { 2, (1.25f, 2) },
        { 3, (1.5f, 3) },
        { 4, (2f, 4)},
        { 5, (3f, 6)}
    };

    private void OnUpgrade(Upgrade upgrade, int newLevel)
    {
        if (upgradeData.TryGetValue(newLevel, out var upgradeValues))
        {
            radius = upgradeValues.radius;
            magicProduction.Magic = upgradeValues.magicProduction;
            Debug.Log($"Wizard Tower upgraded to level {level}: radius = {radius} magicProduction = {magicProduction}");
        }
        else
        {
            Debug.LogError($"Invalid upgrade level {newLevel} for Wizard Tower.");
        }
    }

    public override void RevertProduction()
    {
        Debug.Log("Wizard Tower production is being reverted!");
        productionMultiplier = 1f;
    }
    
    public override void IncreaseProduction()
    {
        Debug.Log("Wizard Tower production is being increased!");
        productionMultiplier = 4f;
    }

    public override Resources GetCurrentProduction()
    {
        return magicProduction * productionMultiplier;
    }
}
