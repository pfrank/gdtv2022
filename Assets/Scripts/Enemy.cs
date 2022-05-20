using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] int hitPoints = 10;

    private NavMeshAgent navMeshAgent;
    private GameObject target;
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Target");
        navMeshAgent.destination = target.transform.position;
    }

    private void Update()
    {
        if (TargetReached())
        {
            Debug.Log("Deal Damage to Castle");
            Destroy(gameObject);
        }
        else if (hitPoints <= 0f)
        {
            Debug.Log("Enemy Destroyed!");
            Destroy(gameObject);
        } 
    }

    private bool TargetReached()
    {
        return ((transform.position - target.transform.position).magnitude < 0.1f);
    }
}
