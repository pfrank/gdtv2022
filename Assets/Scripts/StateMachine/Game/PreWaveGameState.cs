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
        GameManager.Instance.UiManager.ShowCountdownPanel();
    }

    public override void Tick()
    {
        timeRemaining -= Time.deltaTime;
        GameManager.Instance.UiManager.SetCountdownSeconds((int)timeRemaining);
        if (timeRemaining <= 0f)
        {
            GameManager.Instance.NextWave();
        }
    }

    public override void Exit()
    {
        GameManager.Instance.UiManager.HideCountdownPanel();
    }
}
