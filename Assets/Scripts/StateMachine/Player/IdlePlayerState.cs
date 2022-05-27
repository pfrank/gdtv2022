using UnityEngine;

public class IdlePlayerState : BasePlayerState
{
    private GameObject selectedObject;

    public IdlePlayerState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Idle Player State ENTERED");
    }

    public override void Tick()
    {
        // TODO: Raycast from mouse, check if there is an enemy beneath, show
        // its health bar if so


        if (Input.GetMouseButton(0))
        {
            selectedObject = GetObjectUnderCursor();
        }

        GameManager.Instance.UiManager.SetSelectedObjectInfo(selectedObject);
    }

    public override void Exit()
    {
        Debug.Log("Idle Player State Exited");
    }

    public GameObject? GetObjectUnderCursor()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        RaycastHit hitInfo;
        if (Physics.Raycast(
                ray,
                out hitInfo,
                Mathf.Infinity
            )
        )
        {
            if (hitInfo.collider.gameObject.tag == "Tower" || hitInfo.collider.gameObject.tag == "Enemy" || hitInfo.collider.gameObject.tag == "Ground")
            {
                return hitInfo.collider.gameObject;
            }
        }
        return null;
    }
}
