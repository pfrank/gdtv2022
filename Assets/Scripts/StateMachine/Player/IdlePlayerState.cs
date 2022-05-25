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
        // TODO: Raycast from mouse, check if there is an enemy beneath, show
        // its health bar if so

        if (Input.GetMouseButton(0))
        {
            Tower selectedTower = GetTowerUnderCursor();
            if (selectedTower)
            {
                GameManager.Instance.UiManager.SetTowerInfo(selectedTower);
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("Idle Player State Exited");
    }

    public Tower? GetTowerUnderCursor()
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
            if (hitInfo.collider.gameObject.tag == "Tower")
            {
                return hitInfo.collider.gameObject.GetComponent<Tower>();
            }
        }
        return null;
    }
}
