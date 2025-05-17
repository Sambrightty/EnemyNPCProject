// using UnityEngine;
// using UnityEngine.UI;

// public class HealthSystem : MonoBehaviour
// {
//   public float maxHealth = 100f;
//   public float currentHealth;
//   public Slider healthBar;

//   public bool isPlayer = false;

//   public EnemyGrudgeMemory grudgeMemory;

//   public Image lowHealthOverlay;
//   public AudioSource lowHealthAudio;

//   private bool isLowHealthActive = false;
//   private float lowHealthFlashThreshold => maxHealth * 0.3f;
//   private float flashAlpha = 0.5f;
//   private float fadeSpeed = 2f;


//   void Start()
//   {
//     currentHealth = maxHealth;

//     grudgeMemory = GetComponent<EnemyGrudgeMemory>();

//     if (healthBar != null)
//     {
//       healthBar.maxValue = maxHealth;
//       healthBar.value = currentHealth;
//     }

//     Debug.Log("Health initialized: " + currentHealth);
//   }

//   void Update()
// {
//     HandleLowHealthEffects();
// }


//   public void TakeDamage(float amount)
//   {
//     currentHealth -= amount;
//     currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

//     if (healthBar != null)
//       healthBar.value = currentHealth;

//     Debug.Log("Took damage: " + amount + " | Current health: " + currentHealth);

//     if (currentHealth <= 0)
//     {
//       Debug.Log((isPlayer ? "Player" : "Enemy") + " is Dead!");

//       if (!isPlayer && grudgeMemory != null)
//       {
//         grudgeMemory.IncreaseGrudge();
//       }
//     }
//   }

//   public void Heal(float amount)
//   {
//     currentHealth += amount;
//     currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

//     if (healthBar != null)
//       healthBar.value = currentHealth;

//     Debug.Log("Healed: " + amount + " | Current health: " + currentHealth);
//   }

//   public bool IsDead()
//   {
//     return currentHealth <= 0;
//   }
    
//     private void HandleLowHealthEffects()
// {
//     if (currentHealth <= lowHealthFlashThreshold && !IsDead())
//     {
//         if (!isLowHealthActive)
//         {
//             isLowHealthActive = true;
//             if (lowHealthAudio != null && !lowHealthAudio.isPlaying)
//                 lowHealthAudio.Play();
//         }

//         if (lowHealthOverlay != null)
//         {
//             Color color = lowHealthOverlay.color;
//             color.a = Mathf.Lerp(color.a, flashAlpha, Time.deltaTime * fadeSpeed);
//             lowHealthOverlay.color = color;
//         }
//     }
//     else
//     {
//         if (isLowHealthActive)
//         {
//             isLowHealthActive = false;
//             if (lowHealthAudio != null && lowHealthAudio.isPlaying)
//                 lowHealthAudio.Stop();
//         }

//         if (lowHealthOverlay != null)
//         {
//             Color color = lowHealthOverlay.color;
//             color.a = Mathf.Lerp(color.a, 0f, Time.deltaTime * fadeSpeed);
//             lowHealthOverlay.color = color;
//         }
//     }
// }

// }

using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthBar;

    [Header("Low Health Effects")]
    public float lowHealthThreshold = 30f;
    public Image lowHealthOverlay;
    public float flashAlpha = 0.7f;
    public float fadeSpeed = 2f;
    public AudioSource lowHealthAudio;

    private bool isLowHealth = false;
    private Color overlayColor;

    public bool isPlayer = false;

    void Start()
    {
    // currentHealth = maxHealth;
        currentHealth = maxHealth * 0.2f; // Simulate low health
        if (healthBar != null)
    {
      healthBar.maxValue = maxHealth;
      healthBar.value = currentHealth;
    }

        if (lowHealthOverlay != null)
        {
            overlayColor = lowHealthOverlay.color;
            overlayColor.a = 0f;
            lowHealthOverlay.color = overlayColor;
        }

        if (lowHealthAudio != null)
        {
            lowHealthAudio.loop = true;
            lowHealthAudio.Stop();
        }
    }

    void Update()
    {
        HandleLowHealthEffects();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthBar != null)
            healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Debug.Log((isPlayer ? "Player" : "Enemy") + " is Dead!");
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthBar != null)
            healthBar.value = currentHealth;
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    private void HandleLowHealthEffects()
    {
        bool nowLow = currentHealth <= lowHealthThreshold;

        if (nowLow)
        {
            // Flash overlay
            if (lowHealthOverlay != null)
            {
                float alpha = Mathf.PingPong(Time.time * fadeSpeed, flashAlpha);
                overlayColor.a = alpha;
                lowHealthOverlay.color = overlayColor;
            }

            // Play heartbeat
            if (lowHealthAudio != null && !lowHealthAudio.isPlaying)
            {
                lowHealthAudio.Play();
            }
        }
        else
        {
            // Remove overlay
            if (lowHealthOverlay != null)
            {
                overlayColor.a = Mathf.Lerp(lowHealthOverlay.color.a, 0f, Time.deltaTime * fadeSpeed);
                lowHealthOverlay.color = overlayColor;
            }

            // Stop heartbeat
            if (lowHealthAudio != null && lowHealthAudio.isPlaying)
            {
                lowHealthAudio.Stop();
            }
        }

        isLowHealth = nowLow;
    }
}
