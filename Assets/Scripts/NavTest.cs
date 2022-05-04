using UnityEngine;
using UnityEngine.AI;

public class NavTest : MonoBehaviour
{
    public GameObject target;
    NavMeshAgent navMeshAgent;
    
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.destination = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

   
    void Update()
    {
    }
}
