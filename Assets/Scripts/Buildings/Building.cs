using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    MainTower,
    Building0
}
public abstract class Building
{
    public abstract string name { get; }
    public abstract BuildingType type { get; }

    public static Building GetBuilding(BuildingType type)
    {
        switch (type)
        {
            case BuildingType.MainTower: 
                return new MainTower();
            case BuildingType.Building0:
                return new Building0();
            default: 
                return null;
        }
    }
}
