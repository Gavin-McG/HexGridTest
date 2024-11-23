using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTower : Building
{
    [SerializeField] public int buildRange = 5;
    
    void Start()
    {
        UpgradeEvent.AddListener(OnUpgrade);
        rm.RegisterBuilding(this, new Resources(1,0, 0, 0));
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
    }
    
    public override void IncreaseProduction()
    {
        Debug.Log("Main Tower production is being increased!");
    }
}
