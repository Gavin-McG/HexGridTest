using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardTower : Building
{
    public override BuildingType type 
    { 
        get
        {
            return BuildingType.WizardTower;
        }
    }
}
