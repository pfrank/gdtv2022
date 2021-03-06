using System.Collections;
using UnityEngine;

public class LaserWeapon : Weapon
{
    private LineRenderer lineRender;
    private Light muzzleFlash;

    Transform laserOrigin;
    Tower tower;

    void Start()
    {
        laserOrigin = transform.Find("Head/Muzzle");
        muzzleFlash = gameObject.GetComponentInChildren<Light>();
        muzzleFlash.enabled = false;
        lineRender = GetComponentInChildren<LineRenderer>();
        lineRender.enabled = false;
        tower = GetComponent<Tower>();
    }

    public override void Attack(GameObject target)
    {
        if (target == null)
            return;

        tower.CanTarget = false;
        muzzleFlash.enabled = true;
        lineRender.enabled = true;
        lineRender.SetPosition(0, laserOrigin.localPosition);
        lineRender.SetPosition(1, laserOrigin.localPosition + (Vector3.forward * tower.AttackRange));

        RaycastHit[] hits;
        Vector3 rayOrigin = new Vector3(laserOrigin.position.x, laserOrigin.position.y / 2, laserOrigin.position.z);
        Vector3 direction = target.transform.position - rayOrigin;
        Debug.DrawRay(rayOrigin, direction, Color.green, 5f);
        hits = Physics.RaycastAll(rayOrigin, direction, tower.AttackRange);

        foreach (RaycastHit hit in hits)
        {
            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            if (enemy != null && enemy.Alive)
                enemy.TakeDamage(tower, tower.Damage);

        }

        StartCoroutine(DisableLaserEffects());
    }

    private IEnumerator DisableLaserEffects()
    {
        yield return new WaitForSeconds(0.35f);
        tower.CanTarget = true;
        lineRender.enabled = false;
        muzzleFlash.enabled = false;
    }
}
