using UnityEngine;
using UnityEngine.EventSystems;

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
            // Ignore any clicks on the UI
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            GameObject objUnderCursor = GetObjectUnderCursor();

            GameManager.Instance.UiManager.ClearSelection();
            if (objUnderCursor)
                stateMachine.SwitchState(new ObjectSelectedState(stateMachine, objUnderCursor));
            else
                stateMachine.SwitchState(new IdlePlayerState(stateMachine));
        }

    }

    public override void Exit()
    {
        selectedObject.GetComponent<ISelectable>().Deselect();
    }
}