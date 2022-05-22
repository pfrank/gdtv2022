using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] GameObject spawnPoint;
    [SerializeField] List<EnemyWave> waves;

    public List<EnemyWave> Waves { get { return waves; } }

    public void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, spawnPoint.transform);
    }
}
