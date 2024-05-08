using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;

public class SpawnCoinFactory : Factory
{
    [SerializeField] private SpawnCoins spawnCoinPrefab;
    [SerializeField] private Transform spawnCoinHoldingObject;

    public static SpawnCoinFactory Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        
    }

    public override ISpawnItem GetSpawnItem(Vector3 spawnPosition, Vector3 positionToJumpOut)
    {
        GameObject instance = Instantiate(spawnCoinPrefab.gameObject, spawnPosition, Quaternion.Euler(90, 0 ,0), spawnCoinHoldingObject);
        
        SpawnCoins spawnCoin = instance.GetComponent<SpawnCoins>();

        spawnCoin.InitializeItem(positionToJumpOut);

        return spawnCoin;
    }
}
