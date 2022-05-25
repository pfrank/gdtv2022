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

        gameStateMachine.SwitchState(new PreWaveGameState(gameStateMachine, timeBetweenWaves));
        playerStateMachine.SwitchState(new IdlePlayerState(playerStateMachine));
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
        StartWave();
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
}
