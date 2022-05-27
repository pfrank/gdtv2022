using UnityEngine;

public class Projectile : MonoBehaviour, IPausable
{
    [SerializeField] float speed = 10f;
    [SerializeField] int damage = 10;
    [SerializeField] float lifeTime = 5f;

    private Rigidbody rigidBody;
    private GameObject target;
    private bool paused = false;

    public Tower FiredBy
    {
        get; set;
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (paused)
            return;

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
            Destroy(this);

        FollowTarget();
    }

    public void SetTarget(GameObject gobj)
    {
        target = gobj;
    }

    private void FollowTarget()
    {
        if (target != null)
        {
            transform.LookAt(target.transform);
        }
        rigidBody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
            enemy.TakeDamage(FiredBy, damage);

        Destroy(gameObject);
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
