using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyList;
    [SerializeField] float timeBetweenSpawns = 1f;

    public float TimeBetweenSpawns
    {
        get {
            return timeBetweenSpawns;
        }
    }

    public List<GameObject> EnemyList
    {
        get
        {
            return enemyList;
        }
    }
}
