using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthBar;
    public bool isPlayer = false;

    public bool IsBlocking { get; set; }

    public EnemyGrudgeMemory grudgeMemory;

    [Header("Low Health Warning")]
    public Image lowHealthOverlay;
    public AudioSource lowHealthAudio;
    public float lowHealthThreshold = 30f;
    private bool hasPlayedLowHealthCue = false;
    public float fadeSpeed = 1f;
    private Color transparentRed = new Color(1, 0, 0, 0f);
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
            lowHealthOverlay.enabled = false;
        }
    }

    void Update()
    {
        HandleLowHealthOverlay();
    }

    private void HandleLowHealthOverlay()
    {
        if (lowHealthOverlay == null) return;

        if (currentHealth <= lowHealthThreshold)
        {
            lowHealthOverlay.color = Color.Lerp(lowHealthOverlay.color, visibleRed, Time.deltaTime * fadeSpeed);
        }
        else
        {
            lowHealthOverlay.color = Color.Lerp(lowHealthOverlay.color, transparentRed, Time.deltaTime * fadeSpeed);
        }
    }

    public void TakeDamage(float amount)
    {
        if (IsBlocking)
        {
            Debug.Log((isPlayer ? "Player" : "Enemy") + " blocked the attack. No damage taken.");
            return;
        }

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log((isPlayer ? "Player" : "Enemy") + " took damage: " + amount + ", health now: " + currentHealth);

        if (healthBar != null)
            healthBar.value = currentHealth;

        HandleLowHealthEffects();

        if (currentHealth <= 0)
        {
            Debug.Log((isPlayer ? "Player" : "Enemy") + " is Dead!");

            UIManager ui = FindFirstObjectByType<UIManager>();
            if (ui != null)
            {
                if (isPlayer)
                    ui.EndGame("Enemy");
                else
                    ui.EndGame("Player");
            }

            DisableMovement();
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

        HandleLowHealthEffects();
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    private void HandleLowHealthEffects()
    {
        bool isLow = currentHealth <= lowHealthThreshold;

        if (lowHealthOverlay != null)
            lowHealthOverlay.enabled = isLow;

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
                lowHealthAudio.Stop();
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
