using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public enum BuildingType
{
    MainTower,
    Contractor,
    Smithy,
    Farm,
    Tavern,
    WizardTower,
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
    protected ResourceManager rm; //Checks if an upgrade can be bought

    public virtual void Awake()
    {
        rm = FindObjectOfType<ResourceManager>();
    }

    public abstract BuildingType type { get; }

    public string buildingName;
    public bool canDestroy = true; 
    public Resources buildCost;
    protected int level = 0;
    public Upgrade[] upgrades = new Upgrade[0];
    public Structure currentStructure;
    public string descriptionText;
    protected UnityEvent<Upgrade, int> UpgradeEvent = new UnityEvent<Upgrade, int>();
    [HideInInspector] public Vector3Int offsetCoord = Vector3Int.zero;
    
    
    //UI will call this for each building, checks if the player can purchase it
    public virtual void UpgradeBuilding()
    {
        if (level < upgrades.Length)
        {
            Upgrade upgrade = upgrades[level];

            if (rm.CanAfford(upgrade.upgradeCost))
            {
                rm.Charge(upgrade.upgradeCost);
                UpgradeEvent.Invoke(upgrade, level);

                currentStructure = upgrade.newStructure;
                level++;
            }
        }
    }
}
