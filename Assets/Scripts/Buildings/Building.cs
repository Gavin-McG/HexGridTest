using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    MainTower,
    Building0
}
public abstract class Building : MonoBehaviour
{
    public abstract string buildingName { get; }
    public abstract BuildingType type { get; }

    public Resources buildCost;
}
