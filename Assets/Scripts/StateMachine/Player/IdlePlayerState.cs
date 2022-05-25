using UnityEngine;

public class IdlePlayerState : BasePlayerState
{
    public IdlePlayerState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Idle Player State ENTERED");
    }

    public override void Tick()
    {
        // TODO: Raycast from mouse, check if there is an object beneath, show
        // its health bar if so
    }

    public override void Exit()
    {
        Debug.Log("Idle Player State Exited");
    }
}
