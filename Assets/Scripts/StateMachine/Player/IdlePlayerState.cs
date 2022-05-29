using UnityEngine;

public class IdlePlayerState : BasePlayerState
{
    private GameObject selectedObject;

    public IdlePlayerState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
    }

    public override void Tick()
    {
        // TODO: Raycast from mouse, check if there is an enemy beneath, show
        // its health bar if so


        if (Input.GetMouseButtonDown(0))
        {
            selectedObject = GetObjectUnderCursor();
        }

        if (selectedObject)
            stateMachine.SwitchState(new ObjectSelectedState(stateMachine, selectedObject));
    }

    public override void Exit()
    {
    }

}
