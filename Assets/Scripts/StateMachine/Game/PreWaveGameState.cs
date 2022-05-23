using UnityEngine;

public class PreWaveGameState : BaseGameState
{

    private float countDown;
    private float timeRemaining;

    public PreWaveGameState(GameStateMachine stateMachine, float countDown) : base(stateMachine) {
        this.countDown = countDown;
        timeRemaining = countDown;
    }

    public override void Enter()
    {
        // Set the countdown
        Debug.Log("PreWave State ENTERED");
    }

    public override void Tick()
    {
        timeRemaining -= Time.deltaTime;
        // adjust the countdown UI element
        Debug.Log($"Wave Starts in {System.Math.Round(timeRemaining, 0)}");
        if (timeRemaining <= 0f)
        {
            GameManager.Instance.NextWave();
        }
    }

    public override void Exit()
    {
        Debug.Log("PreWave State EXITED");
    }
}
