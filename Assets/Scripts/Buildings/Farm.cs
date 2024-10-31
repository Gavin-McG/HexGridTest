using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Building
{
    private float multiplier = 1.5f;
    private float range = 1.5f;

    public override BuildingType type 
    { 
        get
        {
            return BuildingType.Farm;
        }
    }
}
