using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardTower : Building
{
    [SerializeField] public int buildRange = 4;
    private float radius = 1f;
    private int magicProduction = 1;

    private void Start()
    {
        UpgradeEvent.AddListener(OnUpgrade);
        rm.RegisterBuilding(this, new Resources(magicProduction, 0, 0, 0));
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
            magicProduction = upgradeValues.magicProduction;
            Debug.Log($"Wizard Tower upgraded to level {level}: radius = {radius} magicProduction = {magicProduction}");
        }
        else
        {
            Debug.LogError($"Invalid upgrade level {newLevel} for Wizard Tower.");
        }
    }
}
