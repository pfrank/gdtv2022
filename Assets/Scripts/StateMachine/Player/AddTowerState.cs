using UnityEngine;

public class AddTowerState : BasePlayerState
{
    private GameObject towerObj;
    private GameObject placementObject;
    private SpriteRenderer indicatorRenderer;

    private bool canPlace = false;

    public AddTowerState(PlayerStateMachine stateMachine, GameObject towerObj) : base(stateMachine)
    {
        this.towerObj = towerObj;
    }

    public override void Enter()
    {
        placementObject = GameObject.Find("PlacementObject");
        towerObj.transform.SetParent(placementObject.transform, worldPositionStays: false);
        indicatorRenderer = placementObject.GetComponentInChildren<SpriteRenderer>();
        indicatorRenderer.enabled = true;
    }

    public override void Tick()
    {
        Vector3 placePosition;

        if (GetGroundPositionFromMouse(out placePosition))
        {
            // Place the Sprite Renderer slightly above the ground
            placementObject.transform.position = placePosition + new Vector3(0f, 0.1f, 0f);
            canPlace = CanPlaceAtPosition(placePosition);
            if (canPlace)
            {
                indicatorRenderer.color = Color.green;
                if (Input.GetMouseButtonDown(0))
                {
                    PlaceTower(placePosition);
                }
            }
            else
                indicatorRenderer.color = Color.red;
        }
        else
            canPlace = false;
    }

    public override void Exit()
    {
        indicatorRenderer.enabled = false;
        if (towerObj.transform.parent != null)
            Object.Destroy(towerObj);

    }

    bool GetGroundPositionFromMouse(out Vector3 placePosition)
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        placePosition = new Vector3();
        RaycastHit hitInfo;
        if (Physics.Raycast(
                ray,
                out hitInfo,
                Mathf.Infinity,
                LayerMask.GetMask("Placeable")
            )
        )
        {
            placePosition = hitInfo.point;
            return true;
    }

        return false;
    }

    bool CanPlaceAtPosition(Vector3 position)
    {
        Collider[] hitColliders = Physics.OverlapSphere(
            position,
            placementObject.transform.localScale.x * 0.6f,
            ~LayerMask.GetMask("Placeable")
        );
        return hitColliders.Length == 0;
    }

    void PlaceTower(Vector3 position)
    {
        indicatorRenderer.enabled = false;
        // Turn on colliders
        towerObj.GetComponent<Collider>().enabled = true;
        // Unparent and 
        towerObj.transform.SetParent(parent: null, worldPositionStays: true);
        // Enable Tower script
        Tower tower = towerObj.GetComponent<Tower>();
        tower.enabled = true;
        GameManager.Instance.DeductGold(tower.Cost);

        stateMachine.SwitchState(new IdlePlayerState(stateMachine));
    }
}
