using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();

    void Awake()
    {
        foreach (Transform waypoint in transform.gameObject.GetComponentsInChildren<Transform>())
        {
            // The parent is included this list for some reason, this check
            // ignores it
            if (waypoint.parent != null)
                waypoints.Add(waypoint);
        }
    }
}
