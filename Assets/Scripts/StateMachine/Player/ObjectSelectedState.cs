using UnityEngine;

public class ObjectSelectedState : BasePlayerState
{
    GameObject selectedObject;

    public GameObject SelectedObject
    {
        get
        {
            return selectedObject;
        }
    }

    public ObjectSelectedState(PlayerStateMachine stateMachine, GameObject selectedObject) : base(stateMachine)
    {
        this.selectedObject = selectedObject;
        GameManager.Instance.UiManager.SetSelectedObjectInfo(selectedObject);
    }

    public override void Enter()
    {
        // Turn on the selection indicator
        selectedObject.GetComponent<ISelectable>().Select();
    }

    public override void Tick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject objUnderCursor = GetObjectUnderCursor();

            if (objUnderCursor)
                stateMachine.SwitchState(new ObjectSelectedState(stateMachine, objUnderCursor));
            else
            {
                GameManager.Instance.UiManager.SetSelectedObjectInfo(null);
                stateMachine.SwitchState(new IdlePlayerState(stateMachine));
            }
        }

    }

    public override void Exit()
    {
        // Turn off the selection indicator
        selectedObject.GetComponent<ISelectable>().Deselect();
    }
}