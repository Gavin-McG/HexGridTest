using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.Tilemaps;

public class Farm : Building
{
    private Tilemap environmentTilemap; 
    [SerializeField] private EnvironmentTile treePrefab;
    
    [SerializeField] private float treeSpawnRate = 30f;
    private float spawnTimer;
    
    private float multiplier = 1.5f;
    public int range = 5;

    private Dictionary<int, float> upgradeData = new()
    {
        {1, 1.5f},
        {2, 2f},
        {3, 3f},
        {4, 4f},
        {5, 5f}
    };
    
    public override BuildingType type => BuildingType.Farm;
    
    void Start()
    {
        UpgradeEvent.AddListener(OnUpgrade);
        
        GameObject environmentTilemapObj = GameObject.FindWithTag("ObjectTilemap");
        if (environmentTilemapObj != null)
        {
            environmentTilemap = environmentTilemapObj.GetComponent<Tilemap>();
        }
        else
        {
            Debug.LogError("Tile map with 'ObjectTilemap' tag not found!");
        }
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= treeSpawnRate)
        {
            spawnTimer = 0f;
            SpawnTree();
        }
    }

    private void SpawnTree()
    {
        if (RangeManager.Instance == null) return;

        List<Vector3Int> availableTiles = RangeManager.Instance.GetFarmRangeTiles(this);
        if (availableTiles.Count == 0) return;

        Vector3Int spawnTile = availableTiles[Random.Range(0, availableTiles.Count)];

        if (environmentTilemap.GetTile(spawnTile) == null)
        {
            environmentTilemap.SetTile(spawnTile, treePrefab);
        }
    }

    private void OnUpgrade(Upgrade upgrade, int newLevel)
    {
        if (upgradeData.TryGetValue(newLevel, out var upgradeValues))
        {
            multiplier = upgradeValues;
            Debug.Log($"Farm upgraded to level {level}: multiplier = {multiplier}");
        }
        else
        {
            Debug.LogError($"Invalid upgrade level {newLevel} for Farm.");
        }
    }
}
