using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour, IPausable
{

    [SerializeField] string displayName = "Tower";
    [SerializeField] int cost = 100;
    [SerializeField] int damage = 10;
    [SerializeField] float attackRange = 10f;
    [SerializeField] float attackDelay = 1f;
    [SerializeField] float targetingSpeed = 1f;
    [SerializeField] float upgradeCostMultiplier = 2f;
    [SerializeField] float upgradeBonusMultiplier = 1.1f;
    

    private Weapon weapon;
    private float attackWait = 0f;
    private Transform turret;
    private bool paused = false;

    private int damageUpgrades = 0;
    private int attackDelayUpgrades = 0;
    private int attackRadiusUpgrades = 0;

    public int Kills
    {
        get; set;
    }

    public string AttackSpeed
    {
        get
        {
            // Return attacks per second
            return $"{60.0f / (attackDelay * 60.0f)}/sec";
        }
    }

    public int Cost
    {
        get
        {
            return cost;
        }
    }
    public string DisplayName
    {
        get
        {
            return displayName;
        }
    }

    public int Level
    {
        get
        {
            return 1;
        }
    } 
    public int Damage
    {
        get
        {
            return damage;
        }
    } 

    public float AttackDelay
    {
        get
        {
            return attackDelay;
        }
    } 
    public float AttackRange
    {
        get
        {
            return attackRange;
        }
    } 

    public bool CanTarget
    {
        get; set;
    }

    // Start is called before the first frame update
    void Start()
    {
        CanTarget = true;
        turret = transform.Find("Head");
        weapon = gameObject.GetComponentInChildren<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (paused)
            return;

        attackWait -= Time.deltaTime;
        if (CanTarget)
            AttackNearestEnemyInRange();
        //AttackFarthestEnemyInRange();
    }

    private float DistanceAway(GameObject other)
    {
        return Vector3.Distance(transform.position, other.transform.position);
    }

    List<GameObject> GetEnemiesInRange(GameObject[] enemyList)
    {
        List<GameObject> enemiesInRange = new List<GameObject>();

        foreach (GameObject enemy in enemyList)
        {
            if (DistanceAway(enemy) <= attackRange)
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


    GameObject GetFarthestEnemy()
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
                if (DistanceAway(enemy) > DistanceAway(nearest))
                    nearest = enemy;
            }

        }
        return nearest;
    }


    void LookAtTarget(GameObject target)
    {
        if (target == null)
            return;

        Vector3 targetDir = target.transform.position - turret.transform.position;
        Vector3 newDirection = Vector3.RotateTowards(
            turret.transform.forward,
            targetDir,
            targetingSpeed * Time.deltaTime,
            0.0f
        );
        Quaternion lookAtRotation = Quaternion.LookRotation(newDirection);

        lookAtRotation = Quaternion.Euler(turret.transform.rotation.x, lookAtRotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        turret.transform.rotation = lookAtRotation;
    }

    bool Targeted(GameObject target)
    {
        if (target == null)
            return false;

        Vector3 targetDir = target.transform.position - turret.transform.position;
        float angleBetween = Vector3.Angle(new Vector3(targetDir.x, 0, targetDir.z), turret.transform.forward);

        return angleBetween < 5f;
    }

    void AttackFarthestEnemyInRange()
    {
        GameObject farthest = GetFarthestEnemy();

        if (farthest == null)
            return;

        LookAtTarget(farthest);
        Attack(farthest);
    }


    void AttackNearestEnemyInRange()
    {
        GameObject nearest = GetNearestEnemy();

        if (nearest == null)
            return;

        LookAtTarget(nearest);
        if (Targeted(nearest))
            Attack(nearest);
    }

    void Attack(GameObject target)
    {
        if (target == null)
            return;

        if (attackWait <= 0)
        {
            weapon.Attack(target);
            attackWait = attackDelay;
        }
    }

    public void Pause()
    {
        paused = true;
    }

    public void Unpause()
    {
        paused = false;
    }
}
