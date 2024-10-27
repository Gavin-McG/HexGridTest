using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : Building
{
    public override string buildingName
    {
        get { return "Dungeon"; }
    }

    public override BuildingType type 
    { 
        get { return BuildingType.Dungeon; }
    }

    [SerializeField] HexPoint _entrance;

    [HideInInspector]
    public HexPoint entrance
    {
        get
        {
            return new HexPoint(_entrance.cubicCoord + HexUtils.OffsetToCubic(offsetCoord), _entrance.isTop);
        }
    }
}
