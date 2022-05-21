using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int health = 100;

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"{transform.name} took {damage} damaage ({health} health remaining.");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Tower Destroyed, Game Over!");
        Destroy(gameObject);
    }
}
