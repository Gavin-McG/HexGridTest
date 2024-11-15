using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Dungeon : Building
{

    [System.Serializable]
    public struct DungeonLevel
    {
        [SerializeField] public Vector2Int GoldRange;
        [SerializeField] public int fossilTotal;
        [SerializeField] public int difficulty;
    }

    public override BuildingType type 
    { 
        get { return BuildingType.Dungeon; }
    }

    [SerializeField] HexPoint _entrance;
    [SerializeField] public DungeonLevel[] levels;
    [HideInInspector] public int[] collected;
    [HideInInspector] public bool[] completed;

    [HideInInspector]
    public HexPoint entrance
    {
        get
        {
            return new HexPoint(_entrance.cubicCoord + HexUtils.OffsetToCubic(offsetCoord), _entrance.isTop);
        }
    }

    public override void Awake()
    {
        base.Awake();

        collected = new int[levels.Length];
        completed = new bool[levels.Length];
    }
}
