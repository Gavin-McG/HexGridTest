using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTower : Building
{
    [SerializeField] public int buildRange = 5;
    
    public override BuildingType type 
    { 
        get
        {
            return BuildingType.MainTower;
        }
    }
}
