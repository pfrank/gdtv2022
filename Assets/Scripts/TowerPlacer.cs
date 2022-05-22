using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Material canPlaceMaterial;
    [SerializeField] Material cantPlaceMaterial;

    private GameObject objectToPlace;
    private Transform indicator;

    private int placeableMask;
    private Renderer renderer;

    private bool canPlace = false;

    private bool showGizmos = false;

    // Start is called before the first frame update
    void Start()
    {
        showGizmos = true;
        placeableMask = LayerMask.NameToLayer("Placeable");
        objectToPlace = Instantiate(prefab, transform);
        indicator = transform.Find("Indicator");
        renderer = indicator.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 placePosition;

        if (GetGroundPositionFromMouse(out placePosition))
        {
            transform.position = placePosition;
            canPlace = CanPlaceAtPosition(placePosition);
            if (canPlace)
                renderer.material = canPlaceMaterial;
            else
                renderer.material = cantPlaceMaterial;
        }
        else
            canPlace = false;
    }

    bool GetGroundPositionFromMouse(out Vector3 placePosition)
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        placePosition = new Vector3();
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            placePosition = hitInfo.point;
            if (hitInfo.collider.gameObject.name == "Ground")
                return true;
    }

        return false;
    }

    bool CanPlaceAtPosition(Vector3 position)
    {

        Collider[] hitColliders = Physics.OverlapBox(
            position,
            indicator.transform.localScale,
            Quaternion.identity
        );
        if (hitColliders.Length > 0)
            Debug.Log("BLOCKED");
        return hitColliders.Length == 0;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (showGizmos)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(indicator.transform.position, indicator.transform.localScale);
    }


    void ObjectToMousePosition(GameObject obj)
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
                Vector3 spawnPoint = hitInfo.point; 
                Instantiate(obj, spawnPoint, Quaternion.identity);
        }
    }

}
