using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public KeyCode punchKey = KeyCode.Space;
    public KeyCode blockKey = KeyCode.B;

    public float punchRange = 2f;
    public float punchDamage = 10f;
    public Transform punchOrigin;
    public LayerMask enemyLayer;

    public float healRate = 10f;
    public float healCooldown = 5f;
    private float lastCombatTime = -10f;

    private int punchCount = 0;

    private bool isHealing = false;

    private Rigidbody rb;
    private bool isBlocking = false;

    private HealthSystem healthSystem;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        healthSystem = GetComponent<HealthSystem>();
    }

    void Update()
    {
        Move();

        if (Input.GetKeyDown(punchKey))
        {
            Punch();
            lastCombatTime = Time.time;
        }

        if (Input.GetKeyDown(blockKey))
        {
            isBlocking = true;
            if (healthSystem != null)
                healthSystem.IsBlocking = true;

            lastCombatTime = Time.time;
            Debug.Log("🛡️ Player is blocking.");
        }

        if (Input.GetKeyUp(blockKey))
        {
            isBlocking = false;
            if (healthSystem != null)
                healthSystem.IsBlocking = false;

            Debug.Log("🚫 Player stopped blocking.");
        }

        if (Input.GetKey(KeyCode.H) && Time.time - lastCombatTime > healCooldown)
        {
            if (!isHealing)
                StartCoroutine(PlayerHealOverTime());
        }
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(h, 0, v).normalized;
        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
    }

   void Punch()
{
    RaycastHit hit;
    Vector3 origin = punchOrigin.position;
    Vector3 direction = transform.forward;

    Debug.DrawRay(origin, direction * punchRange, Color.red, 1f); // visualize

    if (Physics.Raycast(origin, direction, out hit, punchRange, enemyLayer))
    {
        Debug.Log("👊 Punch hit: " + hit.collider.name);

        // ✅ FIX: Get HealthSystem from parent if hit object is child (like 'Hitbox')
        HealthSystem enemyHealth = hit.collider.GetComponentInParent<HealthSystem>();
        if (enemyHealth != null)
        {
            if (!enemyHealth.IsBlocking)
            {
                enemyHealth.TakeDamage(punchDamage);
                punchCount++;
                Debug.Log("✅ Enemy hit! Health reduced.");
            }
            else
            {
                Debug.Log("🛡️ Enemy blocked the punch!");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ Hit object has no HealthSystem: " + hit.collider.name);
        }
    }
    else
    {
        Debug.Log("❌ Punch missed.");
    }
}


    public bool IsBlocking()
    {
        return isBlocking;
    }

    private IEnumerator PlayerHealOverTime()
    {
        isHealing = true;
        HealthSystem health = GetComponent<HealthSystem>();

        while (Input.GetKey(KeyCode.H) && health.currentHealth < health.maxHealth)
        {
            health.Heal(healRate * Time.deltaTime);
            yield return null;
        }

        isHealing = false;
    }
}
