using UnityEngine;
using UnityEngine.AI;
using System.Collections;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStateManager))]
public class EnemyFSM : MonoBehaviour
{
    public Transform player;
    public FieldOfView fieldOfView;
    public HealthSystem healthSystem;
    public PlayerBehaviorTracker behaviorTracker;

    private EnemyStateManager stateManager;
    private NavMeshAgent agent;
    private GeneticEnemyAI enemyAI;
    private EnemyGrudgeMemory grudgeMemory;

    [Header("State Thresholds")]
    public float chaseDistance = 10f;
    public float attackDistance = 2f;
    public float lowHealthThreshold = 30f;

    [Header("Search Settings")]
    public float searchDuration = 5f;
    private float searchTimer = 0f;
    private bool isSearching = false;

    private float attackCooldown = 1.5f;
    private float attackTimer = 0f;

    private bool isRetreating = false;
    private bool isHealing = false;
    private float healingRate = 5f; // Health per second
    private float safeDistance = 8f; // Distance to consider safe
    private Vector3 retreatTarget;

    private void Start()
    {
        stateManager = GetComponent<EnemyStateManager>();
        agent = GetComponent<NavMeshAgent>();
        grudgeMemory = GetComponent<EnemyGrudgeMemory>();
        enemyAI = GetComponent<GeneticEnemyAI>();

        if (grudgeMemory != null)
        {
            int grudge = grudgeMemory.GetGrudgeLevel();
            attackDistance += grudge * 0.5f;
            chaseDistance += grudge * 1f;
            agent.speed += grudge * 0.2f;

            Debug.Log("Grudge-enhanced aggression: attackDistance = " + attackDistance + ", chaseDistance = " + chaseDistance);
        }
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool canSeePlayer = fieldOfView.CanSeePlayer;

        if (healthSystem.currentHealth < lowHealthThreshold)
        {
            stateManager.SetState(EnemyStateManager.EnemyState.Retreat);
            Retreat();
        }
        else if (canSeePlayer && distanceToPlayer <= attackDistance)
        {
            isSearching = false;
            stateManager.SetState(EnemyStateManager.EnemyState.Attack);
            Attack();
        }
        else if (canSeePlayer && distanceToPlayer <= chaseDistance)
        {
            isSearching = false;
            stateManager.SetState(EnemyStateManager.EnemyState.Chase);
            Chase();
        }
        else if (!canSeePlayer && (stateManager.currentState == EnemyStateManager.EnemyState.Chase || stateManager.currentState == EnemyStateManager.EnemyState.Attack))
        {
            StartSearch();
        }
        else if (isSearching)
        {
            searchTimer -= Time.deltaTime;
            if (searchTimer <= 0f)
            {
                isSearching = false;
                stateManager.SetState(EnemyStateManager.EnemyState.Patrol);
            }
        }
    }

    private void Chase()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }

        VoiceManager vm = GetComponent<VoiceManager>();
        if (vm != null)
        {
            vm.PlayAlertedVoice();
        }
    }

    private void Attack()
    {
        agent.SetDestination(transform.position); // Stop moving
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;

            string action = enemyAI.DecideNextAction();
            Debug.Log("🔁 [AI Decision] Action: " + action);

            switch (action)
            {
                case "Attack":
                    if (Vector3.Distance(transform.position, player.position) <= attackDistance)
                    {
                        HealthSystem playerHealth = player.GetComponent<HealthSystem>();
                        if (playerHealth != null && !playerHealth.IsBlocking)
                        {
                            playerHealth.TakeDamage(10f);
                            Debug.Log("👊 Enemy punches the player!");
                        }
                        else
                        {
                            Debug.Log("🛡️ Player blocked the punch!");
                        }
                    }
                    break;

                case "Block":
                    Debug.Log("🛡️ Enemy blocks!");
                    healthSystem.IsBlocking = true;
                    Invoke("StopBlocking", 1f);
                    break;

                case "Dodge":
                    Debug.Log("🌀 Enemy dodges!");
                    // Optional: Implement dodge movement
                    break;

                default:
                    Debug.Log("😐 Enemy does nothing.");
                    break;
            }

            // Behavior-adaptive learning
            if (behaviorTracker != null)
            {
                string behavior = behaviorTracker.GetBehaviorType();
                if (!string.IsNullOrEmpty(behavior))
                {
                    enemyAI.EvolveGene(behavior);
                    behaviorTracker.ResetBehavior();
                }
            }
        }
    }

    private void Retreat()
    {
        if (!isRetreating)
        {
            Vector3 directionAway = (transform.position - player.position).normalized;
            retreatTarget = transform.position + directionAway * 10f;

            if (NavMesh.SamplePosition(retreatTarget, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                isRetreating = true;

                VoiceManager vm = GetComponent<VoiceManager>();
                if (vm != null) vm.PlayRetreatVoice();
            }
        }
        else
        {
            float distance = Vector3.Distance(transform.position, retreatTarget);

            // Reached retreat point
            if (distance < 1.5f && !isHealing)
            {
                isHealing = true;
                StartCoroutine(HealOverTime());
            }
        }
    }

    private void StartSearch()
    {
        stateManager.SetState(EnemyStateManager.EnemyState.Search);
        Search();
    }

    private void Search()
    {
        if (!isSearching)
        {
            isSearching = true;
            searchTimer = searchDuration;
            Debug.Log("🟠 Enemy is searching for the player...");
        }
    }

    private void StopBlocking()
    {
        healthSystem.IsBlocking = false;
    }
    

    private IEnumerator HealOverTime()
{
    Debug.Log("💊 Enemy starts healing...");

    while (healthSystem.currentHealth < healthSystem.maxHealth * 0.6f)
    {
        healthSystem.Heal(healingRate * Time.deltaTime);
        yield return null;
    }

    Debug.Log("✅ Enemy healed, back to patrol!");
    isHealing = false;
    isRetreating = false;

    stateManager.SetState(EnemyStateManager.EnemyState.Patrol);
}

}
