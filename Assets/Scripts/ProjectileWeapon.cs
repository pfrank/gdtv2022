using UnityEngine;

public class ProjectileWeapon : Weapon
{
    [SerializeField] GameObject projectile;

    Transform projectileOrigin;

    // Start is called before the first frame update
    void Start()
    {
        projectileOrigin = transform.Find("Muzzle");
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void Attack(GameObject target)
    {
        Projectile instantiated = Instantiate(
            projectile,
            projectileOrigin.transform.position,
            projectile.transform.rotation
        ).GetComponent<Projectile>();
        instantiated.SetTarget(target);
    }
}
