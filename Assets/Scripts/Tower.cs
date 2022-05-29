using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour, IPausable, ISelectable
{

    [SerializeField] string displayName = "Tower";
    [SerializeField] int cost = 100;
    int damage;
    float attackDelay;
    float attackRange;
    [SerializeField] float targetingSpeed = 1f;
    [SerializeField] Upgrades damageUpgrade;
    [SerializeField] Upgrades attackDelayUpgrade;
    [SerializeField] Upgrades rangeUpgrade;
    private int damageUpgradeLevel = 0;
    private int attackDelayUpgradeLevel = 0;
    private int rangeUpgradeLevel = 0;

    private Weapon weapon;
    private float attackWait = 0f;
    private Transform turret;
    private bool paused = false;

    private Transform selectionIndicator;
    private bool selected = false;

    public bool IsDamageMaxLevel
    {
        get
        {
            return damageUpgradeLevel >= damageUpgrade.UpgradeList.Length;
        }
    }

    public bool IsSpeedMaxLevel
    {
        get
        {
            return attackDelayUpgradeLevel >= attackDelayUpgrade.UpgradeList.Length;
        }
    }

    public bool IsRangeMaxLevel
    {
        get
        {
            return rangeUpgradeLevel >= rangeUpgrade.UpgradeList.Length;
        }
    }

    public int Kills
    {
        get; set;
    }

    public string AttackSpeed
    {
        get
        {
            // Return attacks per second
            return $"{60.0f / (attackDelay * 60.0f):.#}/sec";
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
        selectionIndicator = transform.Find("SelectionIndicator");
        weapon = gameObject.GetComponentInChildren<Weapon>();
        UpgradeDamage();
        UpgradeSpeed();
        UpgradeRange();
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

    public void Select()
    {
        // TODO: the constant multiplcation is a hack because scale is not
        // equivalent to radius of the selectionIndicator
        selectionIndicator.localScale = Vector3.one * (attackRange * 1.5f);
        selectionIndicator.GetComponent<SpriteRenderer>().enabled = true;
        selected = true;
    }

    public void Deselect()
    {
        selectionIndicator.GetComponent<SpriteRenderer>().enabled = false;
        selected = false;
    }

    public void UpgradeDamage()
    {
        UpgradeEntry upgrade = damageUpgrade.UpgradeList[damageUpgradeLevel];
        if (upgrade.Cost <= GameManager.Instance.Gold && !IsDamageMaxLevel)
        {
            GameManager.Instance.DeductGold(upgrade.Cost);
            int newValue = (int)upgrade.NewValue;
            damage = newValue;
            damageUpgradeLevel += 1;
            GameManager.Instance.UiManager.UpdateSelectedObjectInfo();
            if (IsDamageMaxLevel)
            {
                GameManager.Instance.UiManager.HideUpgradeButton("Damage");
            }
            else
            {
                UpgradeEntry nextUpgrade = damageUpgrade.UpgradeList[damageUpgradeLevel];
                GameManager.Instance.UiManager.SetUpgradeButtonText("Damage", $"+{nextUpgrade.NewValue - upgrade.NewValue} ({nextUpgrade.Cost}GP)");
            }
        }
    }

    public void UpgradeSpeed()
    {
        UpgradeEntry upgrade = attackDelayUpgrade.UpgradeList[attackDelayUpgradeLevel];
        if (upgrade.Cost <= GameManager.Instance.Gold && !IsSpeedMaxLevel)
        {
            GameManager.Instance.DeductGold(upgrade.Cost);
            int newValue = (int)upgrade.NewValue;
            attackDelay = newValue;
            attackDelayUpgradeLevel += 1;
            GameManager.Instance.UiManager.UpdateSelectedObjectInfo();
            if (IsSpeedMaxLevel)
            {
                GameManager.Instance.UiManager.HideUpgradeButton("Speed");
            }
            else
            {
                UpgradeEntry nextUpgrade = attackDelayUpgrade.UpgradeList[attackDelayUpgradeLevel];
                GameManager.Instance.UiManager.SetUpgradeButtonText("Speed", $"+{nextUpgrade.NewValue - upgrade.NewValue} ({nextUpgrade.Cost}GP)");
            }
        }
    }

    public void UpgradeRange()
    {
        UpgradeEntry upgrade = rangeUpgrade.UpgradeList[rangeUpgradeLevel];
        if (upgrade.Cost <= GameManager.Instance.Gold && !IsRangeMaxLevel)
        {
            GameManager.Instance.DeductGold(upgrade.Cost);
            int newValue = (int)upgrade.NewValue;
            attackRange = newValue;
            selectionIndicator.localScale = Vector3.one * (attackRange * 1.5f);
            rangeUpgradeLevel += 1;
            GameManager.Instance.UiManager.UpdateSelectedObjectInfo();
            if (IsRangeMaxLevel)
            {
                GameManager.Instance.UiManager.HideUpgradeButton("Range");
            }
            else
            {
                UpgradeEntry nextUpgrade = rangeUpgrade.UpgradeList[rangeUpgradeLevel];
                GameManager.Instance.UiManager.SetUpgradeButtonText("Range", $"+{nextUpgrade.NewValue - upgrade.NewValue} ({nextUpgrade.Cost}GP)");
            }
        }
    }
}
