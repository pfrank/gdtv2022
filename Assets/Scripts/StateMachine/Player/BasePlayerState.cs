using UnityEngine;

public abstract class BasePlayerState : State
{
    protected PlayerStateMachine stateMachine;

    public BasePlayerState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
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
            if (hitInfo.collider.gameObject.GetComponent<ISelectable>() != null)            {
                return hitInfo.collider.gameObject;
            }
        }
        return null;
    }
}
