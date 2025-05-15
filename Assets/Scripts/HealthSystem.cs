using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    

    private void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("Health initialized: " + currentHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Took damage: " + amount + " | Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("💀 NPC Died");
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Healed: " + amount + " | Current health: " + currentHealth);
    }
}
