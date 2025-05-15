using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public float damage = 10f;
    public Transform player;
    public HealthSystem playerHealth;

    private float lastAttackTime;

    private void Update()
    {
        if (player == null || playerHealth == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange && Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            Debug.Log("👊 Enemy attacked player!");
            playerHealth.TakeDamage(damage);
        }
    }
}
