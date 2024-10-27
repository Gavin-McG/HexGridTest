using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tavern : Building
{
    public override string buildingName
    {
        get { return "Tavern"; }
    }

    public override BuildingType type 
    { 
        get { return BuildingType.Tavern; }
    }

    [SerializeField] HexPoint _exit;
    
    [HideInInspector] public HexPoint exit 
    {
        get
        {
            return new HexPoint(_exit.cubicCoord + HexUtils.OffsetToCubic(offsetCoord), _exit.isTop);
        }
    }
}
