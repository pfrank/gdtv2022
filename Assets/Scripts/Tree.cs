using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] int startingHealth = 100;
    int currentHealth;

    private void Start()
    {
        currentHealth = startingHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        GameManager.Instance.UiManager.SetTreeHealth(currentHealth, startingHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.GameOver();
        Destroy(gameObject);
    }
}
