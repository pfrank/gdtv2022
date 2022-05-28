using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] GameObject spawnPoint;
    [SerializeField] EnemyWave[] waves;

    List<GameObject> activeEnemies;

    public EnemyWave[] Waves { get { return waves; } }
    public int EnemyCount { get { return activeEnemies.Count; } }

    private void Awake()
    {
        activeEnemies = new List<GameObject>();
    }

    public void SpawnEnemy(GameObject enemy)
    {
        GameObject obj = Instantiate(enemy, spawnPoint.transform);
        activeEnemies.Add(obj);
    }

    public void EnemyDestroyed(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
    }
}
