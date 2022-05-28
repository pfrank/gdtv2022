using UnityEngine;

public class Enemy : MonoBehaviour, IPausable
{
    [SerializeField] string displayName = "Tower";
    [SerializeField] int health = 10;
    [SerializeField] int damage = 10;
    [SerializeField] float speed = 10f;

    private GameObject target;
    private Path path;
    private int currWaypointIndex = 0;

    private bool paused = false;

    public string DisplayName
    {
        get
        {
            return displayName;
        }
    }

    public int Damage
    {
        get
        {
            return damage;
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }
    }

    private void Start()
    {
        path = GameObject.Find("Path").GetComponent<Path>();
        target = GameObject.Find("Tree");
        if (target != null)
        {
            // The target is our final waypoint
            path.waypoints.Add(target.transform);
        }

    }

    private void Update()
    {
        if (paused)
            return;

        MoveAlongPath();
    }

    private void MoveAlongPath()
    {
        // There is no target, do not continue
        if (target == null)
            return;

        Vector3 targetPosition = path.waypoints[currWaypointIndex].position;
        // Do not move along the Y axis, just X and Z
        targetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
            );

        transform.LookAt(targetPosition);

        // Waypoint reached, go to the next waypoint
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            currWaypointIndex++;
            if (currWaypointIndex >= path.waypoints.Count)
            {
                TargetReached();
            }
        }
    }

    public void TakeDamage(Tower attacker, int damage)
    {
        health -= damage;
        Debug.Log($"{transform.name} took {damage} damage ({health} health remaining.");

        if (health <= 0)
        {
            Killed(attacker);
        }
    }

    private void Killed(Tower tower)
    {
        tower.Kills += 1;
        Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void TargetReached()
    {
        target.GetComponent<Tree>().TakeDamage(damage);
        Die();
    }

    public void Pause()
    {
        paused = true;
    }

    public void Unpause()
    {
        paused = false;
    }

    private void OnDestroy()
    {
        if(gameObject == GameManager.Instance.UiManager.Selected)
            GameManager.Instance.UiManager.ClearInfo();

        GameManager.Instance.EnemyDestroyed(gameObject);
    }
}
