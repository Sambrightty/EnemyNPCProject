using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

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

    [Header("State Thresholds")]
    public float chaseDistance = 10f;
    public float attackDistance = 2f;
    public float lowHealthThreshold = 30f;

    private EnemyGrudgeMemory grudgeMemory;

    private void Start()
    {
        stateManager = GetComponent<EnemyStateManager>();
        agent = GetComponent<NavMeshAgent>();
        grudgeMemory = GetComponent<EnemyGrudgeMemory>();

        if (grudgeMemory != null)
        {
            int grudge = grudgeMemory.GetGrudgeLevel();

            // Adapt aggression based on grudge level
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

        if (Input.GetKeyDown(KeyCode.H))
        {
            healthSystem.TakeDamage(20f);
        }

        if (healthSystem.currentHealth < lowHealthThreshold)
        {
            stateManager.SetState(EnemyStateManager.EnemyState.Retreat);
            Retreat();
        }
        else if (canSeePlayer && distanceToPlayer <= attackDistance)
        {
            stateManager.SetState(EnemyStateManager.EnemyState.Attack);
            Attack();
        }
        else if (canSeePlayer && distanceToPlayer <= chaseDistance)
        {
            stateManager.SetState(EnemyStateManager.EnemyState.Chase);
            Chase();
        }
        else if (!canSeePlayer && stateManager.currentState == EnemyStateManager.EnemyState.Chase)
        {
            stateManager.SetState(EnemyStateManager.EnemyState.Search);
            Search();
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

        if (behaviorTracker != null)
        {
            string behavior = behaviorTracker.GetBehaviorType();

            switch (behavior)
            {
                case "Aggressive":
                    Debug.Log("🟢 Enemy dodges! Player is aggressive.");
                    // TODO: Trigger dodge animation or repositioning
                    break;

                case "Defensive":
                    Debug.Log("🔴 Enemy charges! Player is defensive.");
                    // TODO: Trigger charge animation or increased speed
                    break;

                default:
                    Debug.Log("🟡 Enemy attacks normally. Player is balanced.");
                    break;
            }

            behaviorTracker.ResetBehavior();
        }
        else
        {
            Debug.LogWarning("⚠️ PlayerBehaviorTracker not assigned in EnemyFSM.");
        }
    }

    private void Retreat()
    {
        Vector3 directionAway = (transform.position - player.position).normalized;
        Vector3 retreatPosition = transform.position + directionAway * 5f;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(retreatPosition, out hit, 5f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        VoiceManager vm = GetComponent<VoiceManager>();
        if (vm != null)
        {
            vm.PlayRetreatVoice();
        }

    }

    private void Search()
    {
        Debug.Log("Searching for player...");
        // Optional: Implement search logic
    }
}
