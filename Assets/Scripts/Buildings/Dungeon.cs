using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Dungeon : Building
{
    public override BuildingType type 
    { 
        get { return BuildingType.Dungeon; }
    }

    [SerializeField] HexPoint _entrance;
    [SerializeField] public float difficulty;

    [HideInInspector]
    public HexPoint entrance
    {
        get
        {
            return new HexPoint(_entrance.cubicCoord + HexUtils.OffsetToCubic(offsetCoord), _entrance.isTop);
        }
    }
}
