using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawn
{
    [SerializeField] float spawnTime;
    [SerializeField] GameObject enemyPrefab;

    public float SpawnTime
    {
        get
        {
            return spawnTime;
        }
    }

    public GameObject EnemyPrefab
    {
        get
        {
            return enemyPrefab;
        }
    }
}

public class EnemyWave : MonoBehaviour
{
    [SerializeField] EnemySpawn[] enemyList;


    public EnemySpawn[] EnemyList
    {
        get
        {
            return enemyList;
        }
    }
}
