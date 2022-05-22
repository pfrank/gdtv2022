using UnityEngine;

public class LaserWeapon : Weapon
{
    [SerializeField] float distance = 10;
    private LineRenderer lineRender;

    void Start()
    {
        lineRender = GetComponent<LineRenderer>();
        lineRender.enabled = false;
    }

    public override void Attack(GameObject target)
    {
        Debug.Log("ZZZAP");
        lineRender.enabled = true;
        lineRender.SetPosition(0, transform.position);
        lineRender.SetPosition(1, transform.position + (Vector3.forward * distance));
    }
}
