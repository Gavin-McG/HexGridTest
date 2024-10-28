using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Building
{
    private float multiplier = 1.5f;

    public override string buildingName
    {
        get
        {
            return "Farm";
        }
    }

    public override BuildingType type 
    { 
        get
        {
            return BuildingType.Farm;
        }
    }
    
    
}
