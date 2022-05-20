using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, 5f);
    }

    private void SpawnEnemy()
    {
        Instantiate(enemy, transform);
    }
}
