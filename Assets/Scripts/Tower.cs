using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    [SerializeField] int damage = 10;
    [SerializeField] float attackRadius = 10f;
    [SerializeField] float attackDelay = 1f;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileOrigin;

    private float attackWait = 0f;
    private Transform turret;

    // Start is called before the first frame update
    void Start()
    {
        turret = transform.Find("Turret");
    }

    // Update is called once per frame
    void Update()
    {
        AttackNearestEnemyInRange();
    }

    private float DistanceAway(GameObject other) {
        return Vector3.Distance(transform.position, other.transform.position);
    }

    List<GameObject> GetEnemiesInRange(GameObject[] enemyList)
    {
        List<GameObject> enemiesInRange = new List<GameObject>();

        foreach (GameObject enemy in enemyList)
        {
            if (DistanceAway(enemy) <= attackRadius)
                enemiesInRange.Add(enemy);
        }

        return enemiesInRange;
    }

    GameObject GetNearestEnemy()
    {
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> enemiesInRange = GetEnemiesInRange(enemyList);
        GameObject nearest = null;
        foreach (GameObject enemy in enemiesInRange)
        {
            if (nearest == null)
                nearest = enemy;
            else
            {
                if (DistanceAway(enemy) < DistanceAway(nearest))
                    nearest = enemy;
            }

        }
        return nearest;
    }

    void AttackNearestEnemyInRange()
    {
        GameObject nearest = GetNearestEnemy();

        if (nearest == null)
            return;

        // TODO slerp to the enemy, only fire when looking at it
        Vector3 targetPosition = nearest.transform.position;
        // Only rotate around the Y axis
        turret.LookAt(new Vector3(targetPosition.x, turret.transform.position.y, targetPosition.z));
        Attack(nearest); 
    }

    void Attack(GameObject target)
    {
        if (attackWait > 0)
        {
            attackWait -= Time.deltaTime;
        }
        else
        {

            Debug.Log($"Attacking {target}");


            Projectile instantiated = Instantiate(projectile, projectileOrigin.transform.position, projectile.transform.rotation).GetComponent<Projectile>();
            instantiated.SetTarget(target);
            attackWait = attackDelay;
        }
    }
}
