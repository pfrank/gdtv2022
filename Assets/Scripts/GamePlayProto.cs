using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayProto : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject enemy;

    void SpawnAtMouse(GameObject obj)
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

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            SpawnAtMouse(obstacle);
        else if (Input.GetMouseButtonDown(1))
            SpawnAtMouse(enemy);

    }
}
