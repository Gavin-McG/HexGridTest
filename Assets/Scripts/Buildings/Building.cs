using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    MainTower,
    Building0
}

[System.Serializable]
public struct Upgrade
{
    public Structure newStructure;
    public Resources upgradeCost;
}

public abstract class Building : MonoBehaviour
{
    public abstract string buildingName { get; }
    public abstract BuildingType type { get; }

    public Resources buildCost;
    public int level = 0;
    public Upgrade[] upgrades = new Upgrade[0];
}
