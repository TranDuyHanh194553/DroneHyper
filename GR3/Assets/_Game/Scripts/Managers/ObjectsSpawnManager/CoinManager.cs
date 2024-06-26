using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : Singleton<CoinManager>
{
    [SerializeField] public int coinNumber;
    void Start()
    {
        for (int i = 0; i < coinNumber; i++)
        {
            CreateCoin();
        }
    }

    private void CreateCoin()
    {
        SimplePool.Spawn<CombatTriggers>(PoolType.Coin, new Vector3(UnityEngine.Random.Range(-90f, 90f), UnityEngine.Random.Range(-3f, 9f), 0), Quaternion.identity);
    }
}
