using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public Resources currentResource;
    public int fossilCount = 0;
    public float refundRate = 1f;
    [SerializeField] float productionRate = 5f;
    public List<Building> productionList = new List<Building>();
    
    public bool CanAfford(Resources cost)
    {
        return cost <= currentResource;
    }

    public void Charge(Resources cost)
    {
        currentResource -= cost;
    }

    public void Refund(Resources cost)
    {
        currentResource += cost * refundRate;
    }

    //I might need an UpdateBuilding for when the buildings are upgraded - CS
    public void RegisterBuilding(Building building, Resources productionAmount)
    {
        productionList.Add(building);
        StartCoroutine(ProduceResources(building, productionAmount));
    }

    private void Produce(Resources production)
    {
        currentResource += production;
    }

    private IEnumerator ProduceResources(Building building, Resources productionAmount)
    {
        while (true)
        {
            yield return new WaitForSeconds(productionRate);
            if (building && building.isActiveAndEnabled)
            {
                Produce(productionAmount);
            }
            else
            {
                yield break; //Stop this coroutine when the building is destroyed or no longer active
            }
        }
    }
}
