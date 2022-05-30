using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float timeBetweenWaves = 10f;
    [SerializeField] private int startingGold = 1000;
    [SerializeField] private string successSubject;
    [SerializeField] private string successMessage;
    [SerializeField] private string failureSubject;
    [SerializeField] private string failureMessage;
    [SerializeField] AudioClip enemyKilled;

    private int currentGold;

    private GameStateMachine gameStateMachine;
    private PlayerStateMachine playerStateMachine;
    private static GameManager instance;

    private WaveManager waveManager;
    private UIManager uiManager;

    [SerializeField] private int currentWave = 0;

    private BaseGameState pausedGameState;

    public BaseGameState GameState
    {
        get
        {
            return (BaseGameState)gameStateMachine.CurrentState;
        }
    }

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public UIManager UiManager
    {
        get
        {
            return uiManager;
        }
    }

    public int Gold
    {
        get
        {
            return currentGold;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        gameStateMachine = GetComponent<GameStateMachine>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    void Start()
    {
        currentGold = startingGold;
        waveManager = GetComponent<WaveManager>();
        uiManager = GetComponent<UIManager>();

        uiManager.SetGold(currentGold);

        playerStateMachine.SwitchState(new IdlePlayerState(playerStateMachine));
        NextWave();
    }

    void Update()
    {
        // Cancel will cancel the players actions unless they are in the idle
        // state, in which case, it will toggle pausing
        if (Input.GetButtonDown("Cancel"))
        {
            if (playerStateMachine.CurrentState is not IdlePlayerState)
                playerStateMachine.SwitchState(new IdlePlayerState(playerStateMachine));
            else
                TogglePauseGame();
        }
    }


    private void TogglePauseGame()
    {
        if (gameStateMachine.CurrentState is PausedGameState)
            gameStateMachine.SwitchState(pausedGameState);
        else
        {
            pausedGameState = GameState;
            gameStateMachine.SwitchState(new PausedGameState(gameStateMachine));
        }
    }

    public void StartWave()
    {
        uiManager.SetWaveNumber(currentWave);
        gameStateMachine.SwitchState(new WaveGameState(gameStateMachine, waveManager, currentWave));
    }

    public void NextWave()
    {
        currentWave += 1;
        if (currentWave > waveManager.Waves.Length)
            WavesComplete();
        else
            gameStateMachine.SwitchState(new PreWaveGameState(gameStateMachine, timeBetweenWaves));
    }

    public void AddGold(int added)
    {
        currentGold += added;
        gameObject.GetComponent<AudioSource>().PlayOneShot(enemyKilled);
        uiManager.SetGold(currentGold);
    }

    public void DeductGold(int deducted)
    {
        currentGold -= deducted;
        uiManager.SetGold(currentGold);
    }

    public void AddTower(GameObject towerPrefab)
    {
        Tower tower = towerPrefab.GetComponent<Tower>();
        if (currentGold < tower.Cost)
            Debug.Log("Not enough gold!");
        else
        {
            GameObject towerObj = Instantiate(towerPrefab, Vector3.zero, Quaternion.identity);
            playerStateMachine.SwitchState(new AddTowerState(playerStateMachine, towerObj));
        }
    }

    public void EnemyDestroyed(GameObject enemy)
    {
        // If this enemy is the currently selected one, deselect it
        if (playerStateMachine.CurrentState is ObjectSelectedState)
        {
            ObjectSelectedState curState = (ObjectSelectedState)playerStateMachine.CurrentState;
            if (curState.SelectedObject == enemy)
                playerStateMachine.SwitchState(new IdlePlayerState(playerStateMachine));
        }
        waveManager.EnemyDestroyed(enemy);
        if (waveManager.EnemyCount == 0)
            NextWave();
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER");
        gameStateMachine.SwitchState(new PausedGameState(gameStateMachine));

        uiManager.ShowGameOverMessage(failureSubject, failureMessage, currentWave-1);
    }

    public void WavesComplete()
    {
        Debug.Log("WAVES COMPLETE!");
        gameStateMachine.SwitchState(new WavesCompleteState(gameStateMachine));

        uiManager.ShowGameOverMessage(successSubject, successMessage, currentWave-1);
    }

    public void RespawnGhost(Enemy enemy)
    {
        if (enemy.GhostForm == null)
            return;

        GameObject ghost = waveManager.SpawnEnemy(enemy.GhostForm);
        ghost.transform.position = enemy.transform.position;
        ghost.GetComponent<Enemy>().CurrWaypointIndex = enemy.CurrWaypointIndex;
    }
}
