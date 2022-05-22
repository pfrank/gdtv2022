using System.Collections;
using UnityEngine;

public class LaserWeapon : Weapon
{
    [SerializeField] float distance = 10;
    private LineRenderer lineRender;
    private Light light;

    Transform laserOrigin;

    void Start()
    {
        laserOrigin = transform.Find("Muzzle");
        light = gameObject.GetComponentInChildren<Light>();
        light.enabled = false;
        lineRender = GetComponent<LineRenderer>();
        lineRender.enabled = false;
    }

    public override void Attack(GameObject target)
    {
        Debug.Log("ZZZAP");
        light.enabled = true;
        lineRender.enabled = true;
        lineRender.SetPosition(0, laserOrigin.localPosition);
        lineRender.SetPosition(1, laserOrigin.localPosition + (Vector3.forward * distance));


        StartCoroutine(DisableLaserEffects());
    }

    private IEnumerator DisableLaserEffects()
    {
        yield return new WaitForSeconds(0.3f);
        lineRender.enabled = false;
        light.enabled = false;
    }
}
