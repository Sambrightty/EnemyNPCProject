// using UnityEngine;
// using UnityEngine.AI;
// using UnityEngine.InputSystem;

// [RequireComponent(typeof(NavMeshAgent))]
// [RequireComponent(typeof(EnemyStateManager))]
// public class EnemyFSM : MonoBehaviour
// {
//     public Transform player;
//     public FieldOfView fieldOfView;
//     public HealthSystem healthSystem;

//     private EnemyStateManager stateManager;
//     private NavMeshAgent agent;
    

//     [Header("State Change Thresholds")]
//     public float chaseDistance = 10f;
//     public float attackDistance = 2f;
//     public float lowHealthThreshold = 30f;

//     private void Start()
//     {
//         stateManager = GetComponent<EnemyStateManager>();
//         agent = GetComponent<NavMeshAgent>();
//     }

//     private void Update()
//     {
//         float distanceToPlayer = Vector3.Distance(transform.position, player.position);

//         bool canSeePlayer = fieldOfView.canSeePlayer;

//         // TEMPORARY TEST
//         if (Keyboard.current.hKey.wasPressedThisFrame)
//         {
//             healthSystem.TakeDamage(20f);
//         }


//         // FSM Transitions
//         if (healthSystem.currentHealth < lowHealthThreshold)
//         {
//             stateManager.SetState(EnemyStateManager.EnemyState.Retreat);
//         }
//         else if (canSeePlayer && distanceToPlayer <= attackDistance)
//         {
//             stateManager.SetState(EnemyStateManager.EnemyState.Attack);
//         }
//         else if (canSeePlayer && distanceToPlayer <= chaseDistance)
//         {
//             stateManager.SetState(EnemyStateManager.EnemyState.Chase);
//         }
//         else if (!canSeePlayer && stateManager.currentState == EnemyStateManager.EnemyState.Chase)
//         {
//             stateManager.SetState(EnemyStateManager.EnemyState.Search);
//         }
//         else if (stateManager.currentState != EnemyStateManager.EnemyState.Patrol)
//         {
//             stateManager.SetState(EnemyStateManager.EnemyState.Patrol);
//         }
//     }
// }


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

    private EnemyStateManager stateManager;
    private NavMeshAgent agent;

    [Header("State Change Thresholds")]
    public float chaseDistance = 10f;
    public float attackDistance = 2f;
    public float lowHealthThreshold = 30f;

    private void Start()
    {
        stateManager = GetComponent<EnemyStateManager>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool canSeePlayer = fieldOfView.canSeePlayer;

        // TEMPORARY TEST
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            healthSystem.TakeDamage(20f);
        }

        // 🔄 FSM Transitions
        if (healthSystem.currentHealth < lowHealthThreshold)
        {
            stateManager.SetState(EnemyStateManager.EnemyState.Retreat);
        }
        else if (canSeePlayer && distanceToPlayer <= attackDistance)
        {
            stateManager.SetState(EnemyStateManager.EnemyState.Attack);
        }
        else if (canSeePlayer && distanceToPlayer <= chaseDistance)
        {
            stateManager.SetState(EnemyStateManager.EnemyState.Chase);
        }
        else if (!canSeePlayer && stateManager.currentState == EnemyStateManager.EnemyState.Chase)
        {
            stateManager.SetState(EnemyStateManager.EnemyState.Search);
        }
        else if (stateManager.currentState != EnemyStateManager.EnemyState.Patrol)
        {
            stateManager.SetState(EnemyStateManager.EnemyState.Patrol);
        }

        // ✅ Handle movement/behavior based on current state
        HandleCurrentState();
    }

    private void HandleCurrentState()
    {
        switch (stateManager.currentState)
        {
            case EnemyStateManager.EnemyState.Chase:
                if (player != null)
                {
                    agent.SetDestination(player.position);
                }

                // Optional: check if already close enough to attack
                if (Vector3.Distance(transform.position, player.position) <= attackDistance)
                {
                    stateManager.SetState(EnemyStateManager.EnemyState.Attack);
                }
                break;

            case EnemyStateManager.EnemyState.Patrol:
                // Patrol logic handled by another component like PatrolSystem
                break;

            case EnemyStateManager.EnemyState.Attack:
                // Stop movement if needed
                agent.SetDestination(transform.position);
                break;

            case EnemyStateManager.EnemyState.Retreat:
                // Implement retreat logic if needed
                break;

            case EnemyStateManager.EnemyState.Search:
                // Implement search logic if needed
                break;
        }
    }
}
