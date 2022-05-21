using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] float timeBetweenSpawns = 1f;



    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, timeBetweenSpawns);
    }

    private void Update()
    {
        
    }

    private void SpawnEnemy()
    {
        Instantiate(enemy, transform);
    }
}
