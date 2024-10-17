using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building0 : Building
{
    public override string buildingName
    {
        get
        {
            return "Building 0";
        }
    }

    public override BuildingType type
    {
        get
        {
            return BuildingType.Building0;
        }
    }
}
