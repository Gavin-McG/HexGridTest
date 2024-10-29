using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum BuildingType
{
    MainTower,
    Tavern,
    Building0,
    Dungeon
}

[System.Serializable]
public struct Upgrade
{
    public Structure newStructure;
    public Resources upgradeCost;
}

public abstract class Building : MonoBehaviour
{
    public abstract BuildingType type { get; }

    public string buildingName;
    public bool canDestroy = true; 
    public Resources buildCost;
    public int level = 0;
    public Upgrade[] upgrades = new Upgrade[0];
    public Structure currentStructure;
    public string descriptionText;

    [HideInInspector] public Vector3Int offsetCoord = Vector3Int.zero;
}
