using UnityEngine;
public class WavesCompleteState : BaseGameState
{
    public WavesCompleteState(GameStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("WavesCompleteState ENTER");
    }

    public override void Exit()
    {
        Debug.Log("WavesCompleteState EXIT");
    }

    public override void Tick()
    {
    }
}
