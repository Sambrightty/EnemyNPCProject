using UnityEngine;

public class EnemyHearing : MonoBehaviour
{
    public FieldOfView fovScript; // Reference to FieldOfView.cs

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Simulate loud action detection
            if (PlayerIsLoud(other))
            {
                Debug.Log("Sound detected! Switching to Suspicious state.");
                fovScript.HearPlayer();
            }
        }
    }

    bool PlayerIsLoud(Collider player)
    {
        // You can later replace this with actual sprint/attack checks.
        // For now, always return true (simulate noise).
        return true;
    }
}
