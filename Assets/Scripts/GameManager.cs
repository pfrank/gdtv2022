using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float timeBetweenWaves = 10f;

    private GameStateMachine gameStateMachine;
    private static GameManager instance;

    private WaveManager waveManager;
    private UIManager uiManager;

    private int currentWave = 0;

    private bool isPaused = false;
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
    }

    // Start is called before the first frame update
    void Start()
    {
        // Start countdown before the Wave starts
        // Set the level (i.e. the number and type of enemies in the wave)
        // Enable the spawnPoint
        //
        waveManager = GetComponent<WaveManager>();
        uiManager = GetComponent<UIManager>();
        gameStateMachine.SwitchState(new PreWaveGameState(gameStateMachine, timeBetweenWaves));
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            TogglePauseGame();
        }
    }


    private void TogglePauseGame()
    {
        if (isPaused)
            gameStateMachine.SwitchState(pausedGameState);
        else
        {
            pausedGameState = GameState;
            gameStateMachine.SwitchState(new PausedGameState(gameStateMachine));
        }

        isPaused = !isPaused;
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
}
