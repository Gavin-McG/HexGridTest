using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTower : Building
{
    [SerializeField] public int buildRange = 5;
    private float productionMultiplier = 1f;
    private Resources magicProduction = new Resources(1, 0, 0, 0);
    
    void Start()
    {
        UpgradeEvent.AddListener(OnUpgrade);
        rm.RegisterBuilding(this);
    }

    public override BuildingType type => BuildingType.MainTower;

    [SerializeField] HexPoint _entrance;

    [HideInInspector]
    public HexPoint entrance => new HexPoint(_entrance.cubicCoord + HexUtils.OffsetToCubic(offsetCoord), _entrance.isTop);

    //TODO: Implement main tower upgrades
    void OnUpgrade(Upgrade upgrade, int newLevel)
    {
        Debug.LogWarning("Upgrade for Main Tower not Implemented!");
    }

    public override void RevertProduction()
    {
        Debug.Log("Main Tower production is being reverted!");
        productionMultiplier = 1f;
    }
    
    public override void IncreaseProduction()
    {
        Debug.Log("Main Tower production is being increased!");
        productionMultiplier = 4f;
    }
    
    public override Resources GetCurrentProduction()
    {
        return magicProduction * productionMultiplier;
    }
}
