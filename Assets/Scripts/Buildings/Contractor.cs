using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contractor : Building
{
    public override string buildingName
    {
        get
        {
            return "Contractor";
        }
    }

    public override BuildingType type 
    { 
        get
        {
            return BuildingType.Contractor;
        }
    }
}
