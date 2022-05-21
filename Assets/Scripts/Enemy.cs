using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health = 10;
    [SerializeField] int damage = 10;
    [SerializeField] float speed = 10f;

    private Path path;
    private int currWaypointIndex = 0;
    private GameObject castle;

    private void Start()
    {
        path = GameObject.Find("Path").GetComponent<Path>();
        castle = GameObject.Find("Castle");
        if (castle != null)
        {
            // The target is our final waypoint
            path.waypoints.Add(castle.transform);
        }

    }

    private void Update()
    {
        MoveAlongPath();
    }

    private void MoveAlongPath()
    {
        // There is no castle, do not continue
        if (castle == null)
            return;

        // Has the target been reached?
        if (transform.position == castle.transform.position)
        {
            TargetReached();
            return;
        }

        if (currWaypointIndex >= path.waypoints.Count)
            return;

        Vector3 targetPosition = path.waypoints[currWaypointIndex].position;
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
            );

        transform.LookAt(targetPosition);

        // Waypoint reached, go to the next waypoint
        if (transform.position == targetPosition)
            currWaypointIndex++;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"{transform.name} took {damage} damage ({health} health remaining.");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy Destroyed!");
        Destroy(gameObject);
    }

    private void TargetReached()
    {
        Debug.Log("Target Reached!");
        castle.GetComponent<Castle>().TakeDamage(damage);
        Destroy(gameObject);
    }
}
