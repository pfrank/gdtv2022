using System.Collections;
using UnityEngine;

public class LaserWeapon : Weapon
{
    [SerializeField] int damage = 10;
    [SerializeField] float distance = 10f;
    private LineRenderer lineRender;
    private Light muzzleFlash;

    Transform laserOrigin;
    Tower tower;

    void Start()
    {
        laserOrigin = transform.Find("Turret/Barrel/Muzzle");
        muzzleFlash = gameObject.GetComponentInChildren<Light>();
        muzzleFlash.enabled = false;
        lineRender = GetComponentInChildren<LineRenderer>();
        lineRender.enabled = false;
        tower = GetComponent<Tower>();
    }

    public override void Attack(GameObject target)
    {
        tower.CanTarget = false;
        muzzleFlash.enabled = true;
        lineRender.enabled = true;
        lineRender.SetPosition(0, laserOrigin.localPosition);
        lineRender.SetPosition(1, laserOrigin.localPosition + (Vector3.forward * distance));

        RaycastHit[] hits;
        Vector3 rayOrigin = new Vector3(laserOrigin.position.x, laserOrigin.position.y / 2, laserOrigin.position.z);
        Vector3 direction = target.transform.position - rayOrigin;
        Debug.DrawRay(rayOrigin, direction, Color.green, 1f);
        hits = Physics.RaycastAll(rayOrigin, direction, distance);

        foreach (RaycastHit hit in hits)
        {
            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(damage);

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
