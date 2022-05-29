using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float timeBetweenWaves = 10f;
    [SerializeField] private int startingGold = 1000;
    private int currentGold;

    private GameStateMachine gameStateMachine;
    private PlayerStateMachine playerStateMachine;
    private static GameManager instance;

    private WaveManager waveManager;
    private UIManager uiManager;

    private int currentWave = 0;

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

    public void AddGold(int deducted)
    {
        currentGold += deducted;
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
        AddGold(enemy.GetComponent<Enemy>().Gold);
        if (waveManager.EnemyCount == 0)
            NextWave();
    }

    public void WavesComplete()
    {
        Debug.Log("WAVES COMPLETE!");
        gameStateMachine.SwitchState(new WavesCompleteState(gameStateMachine));
    }
}
