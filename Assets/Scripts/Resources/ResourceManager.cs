using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


public class ResourceManager : MonoBehaviour
{
    public Resources currentResource;
    public float refundRate = 1f;
    [SerializeField] float productionRate = 5f;
    
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

    public void RegisterBuilding(Building building, Resources productionAmount)
    {
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
