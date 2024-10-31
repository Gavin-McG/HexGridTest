using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smithy : Building
{
    public override BuildingType type 
    { 
        get
        {
            return BuildingType.Smithy;
        }
    }
}
