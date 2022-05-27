using System.Collections;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    [SerializeField] GameObject projectile;

    Tower tower;
    private Light muzzleFlash;
    Transform projectileOrigin;

    // Start is called before the first frame update
    void Start()
    {
        projectileOrigin = transform.Find("Head/Muzzle");
        muzzleFlash = gameObject.GetComponentInChildren<Light>();
        muzzleFlash.enabled = false;
        tower = GetComponent<Tower>();
    }

    public override void Attack(GameObject target)
    {
        muzzleFlash.enabled = true;
        tower.CanTarget = false;
        Projectile instantiated = Instantiate(
            projectile,
            projectileOrigin.transform.position,
            projectile.transform.rotation
        ).GetComponent<Projectile>();
        instantiated.SetTarget(target);
        instantiated.SetFiredBy(tower);

        StartCoroutine(DisableEffects());
    }

    private IEnumerator DisableEffects()
    {
        yield return new WaitForSeconds(0.1f);
        tower.CanTarget = true;
        muzzleFlash.enabled = false;
    }
}
