using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTower : Building
{
    public override string name
    {
        get
        {
            return "Main Tower";
        }
    }

    public override BuildingType type 
    { 
        get
        {
            return BuildingType.MainTower;
        }
    }
}