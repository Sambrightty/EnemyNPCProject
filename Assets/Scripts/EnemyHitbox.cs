using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("👊 Enemy Hit the Player!");
            other.GetComponent<HealthSystem>()?.TakeDamage(10f);
        }
    }
}
