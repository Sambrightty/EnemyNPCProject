
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthBar;

    public bool isPlayer = false;

    public EnemyGrudgeMemory grudgeMemory;

    [Header("Low Health Warning")]
    public Image lowHealthOverlay;
    public AudioSource lowHealthAudio;
    public float lowHealthThreshold = 30f;
    private bool hasPlayedLowHealthCue = false;
    public float fadeSpeed = 1f;
    private Color transparentRed = new Color(1, 0, 0, 0f);   // Fully transparent
    private Color visibleRed = new Color(1, 0, 0, 0.3f);

    private bool hasPlayedHurtVoice = false;

    void Start()
    {
        currentHealth = maxHealth;

        grudgeMemory = GetComponent<EnemyGrudgeMemory>();

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (lowHealthOverlay != null)
        {
            lowHealthOverlay.enabled = false; // hide at start
        }
    }

    void Update()
    {
        if (lowHealthOverlay != null)
        {
            if (currentHealth <= lowHealthThreshold)
            {
                // Fade in
                lowHealthOverlay.color = Color.Lerp(lowHealthOverlay.color, visibleRed, Time.deltaTime * fadeSpeed);
            }
            else
            {
                // Fade out
                lowHealthOverlay.color = Color.Lerp(lowHealthOverlay.color, transparentRed, Time.deltaTime * fadeSpeed);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthBar != null)
            healthBar.value = currentHealth;

        HandleLowHealthEffects();

        if (currentHealth <= 0)
        {
            Debug.Log((isPlayer ? "Player" : "Enemy") + " is Dead!");

            UIManager ui = FindObjectOfType<UIManager>();
            if (ui != null)
            {
                if (isPlayer)
                    ui.EndGame("Enemy");
                else
                    ui.EndGame("Player");
            }

            DisableMovement(); // 👇 new method to stop movement
        }


        if (!isPlayer && currentHealth <= maxHealth * 0.3f && !hasPlayedHurtVoice)
        {
            VoiceManager vm = GetComponent<VoiceManager>();
            if (vm != null)
            {
                vm.PlayHurtVoice();
                hasPlayedHurtVoice = true;
            }
        }



    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthBar != null)
            healthBar.value = currentHealth;

        HandleLowHealthEffects(); // Reset cue if healed
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    void HandleLowHealthEffects()
    {
        bool isLow = currentHealth <= lowHealthThreshold;

        // Flash red screen
        if (lowHealthOverlay != null)
        {
            lowHealthOverlay.enabled = isLow;
        }

        // Play warning sound once
        if (lowHealthAudio != null)
        {
            if (isLow && !hasPlayedLowHealthCue)
            {
                lowHealthAudio.Play();
                hasPlayedLowHealthCue = true;
            }
            else if (!isLow)
            {
                hasPlayedLowHealthCue = false;
                lowHealthAudio.Stop(); // Optional: stop if health recovers
            }
        }
    }
    
    private void DisableMovement()
    {
        if (isPlayer)
        {
            PlayerController controller = GetComponent<PlayerController>();
            if (controller != null) controller.enabled = false;
        }
        else
        {
            EnemyFSM fsm = GetComponent<EnemyFSM>();
            if (fsm != null) fsm.enabled = false;
        }
    }

}
