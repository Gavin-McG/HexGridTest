using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourceManager : MonoBehaviour
{
    public Resources currentResource;

    public bool CanAfford(Resources cost)
    {
        return cost <= currentResource;
    }

    public void Charge(Resources cost)
    {
        currentResource -= cost;
    }
}
