using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTriggersManager : MonoBehaviour
{
    //[SerializeField] private GameObject mutilBulletTriggerPrefab;
    //[SerializeField] private GameObject jumpForceUpTriggerPrefab;
    //[SerializeField] private GameObject slowBulletTriggerPrefab;

    [SerializeField] int triggerNumber = 4;
    void Start()
    {
        for (int i = 0; i < triggerNumber; i++)
        {
            CreateTrigger();
        }
    }

    private void CreateTrigger()
    {
        SimplePool.Spawn<CombatTriggers>(PoolType.MultiBulletTrigger, new Vector3(UnityEngine.Random.Range(-90f, 90f), UnityEngine.Random.Range(-3f, 9f), 0), Quaternion.identity);
        SimplePool.Spawn<CombatTriggers>(PoolType.JumpForceUpTrigger, new Vector3(UnityEngine.Random.Range(-90f, 90f), UnityEngine.Random.Range(-3f, 9f), 0), Quaternion.identity);
        SimplePool.Spawn<CombatTriggers>(PoolType.SlowBulletTrigger, new Vector3(UnityEngine.Random.Range(-90f, 90f), UnityEngine.Random.Range(-3f, 9f), 0), Quaternion.identity);
    }
}
