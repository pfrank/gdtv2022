using UnityEngine;
public class WaveGameState : BaseGameState
{
    private WaveManager waveManager;
    private EnemyWave wave;

    private float spawnWait;
    private int enemyNumber = 0;

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
        Debug.Log("Wave State ENTERED");
    }
    public override void Tick()
    {
        if (enemyNumber < wave.EnemyList.Count)
        {
            spawnWait -= Time.deltaTime;
            if (spawnWait <= 0f)
            {
                spawnWait = wave.TimeBetweenSpawns;
                GameObject enemy = wave.EnemyList[enemyNumber];
                waveManager.SpawnEnemy(enemy);
                enemyNumber++;
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("Wave State EXITED");
    }
}
