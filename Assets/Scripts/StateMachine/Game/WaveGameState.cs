using UnityEngine;
public class WaveGameState : BaseGameState
{
    private WaveManager waveManager;
    private EnemyWave wave;

    private float spawnWait;
    private int enemyNumber = 1;

    public WaveGameState(
        GameStateMachine stateMachine,
        WaveManager waveManager,
        int waveNumber
    ) : base(stateMachine)
    {
        // To simplify design, Waves are 1 based so subtract 1 to get the index
        // into the list properly
        wave = waveManager.Waves[waveNumber - 1];
        this.waveManager = waveManager; 
    }

    public override void Enter()
    {
    }
    public override void Tick()
    {
        if (enemyNumber < wave.EnemyList.Length)
        {
            spawnWait -= Time.deltaTime;
            if (spawnWait <= 0f)
            {
                SpawnNextEnemy();
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("Wave State EXITED");
    }

    private void SpawnNextEnemy()
    {
        EnemySpawn enemySpawn = wave.EnemyList[enemyNumber];
        GameObject enemy = enemySpawn.EnemyPrefab;
        waveManager.SpawnEnemy(enemy);
        enemyNumber++;
        spawnWait = enemySpawn.SpawnTime;
    }
}
