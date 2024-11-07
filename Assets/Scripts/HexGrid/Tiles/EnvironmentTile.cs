using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnvironmentType
{
    Tree
}

[CreateAssetMenu(fileName = "New Environment Tile", menuName = "Tiles/Environment Tile")]
public class EnvironmentTile : BasicTile
{
    [Space(10)]
    [SerializeField] EnvironmentType envType;
    [SerializeField] public string tileName;

    void OnEnable()
    {
        BuildingManager.EnvironmentDeleted.AddListener(OnDestroyTile);
    }

    void OnDestroyTile(EnvironmentTile environmentTile, Vector3Int position)
    {
        switch (environmentTile.envType)
        {
            case EnvironmentType.Tree:
                //Set refund rate to 1 to ensure that you get the same amount of wood every time
                int woodAmount = (int)Math.Round(20 / ResourceManager.Instance.refundRate);
                ResourceManager.Instance.Refund(new Resources(0, woodAmount, 0, 0));
                break;
            default:
                Debug.LogError("envType: " + environmentTile.envType + " does not exist!");
                break;
        }
    }
}
