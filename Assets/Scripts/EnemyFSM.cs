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

    [Header("State Thresholds")]
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
        bool canSeePlayer = fieldOfView.CanSeePlayer;

        // TEMPORARY TEST
        if (Input.GetKeyDown(KeyCode.H)) // Press 'H' to hurt
        {
            healthSystem.TakeDamage(20f);
        }

        // Decide state
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
    }

    private void Attack()
    {
         agent.SetDestination(transform.position); 
        // Add your attack logic (e.g., animation, damage, etc.)
        Debug.Log("Enemy attacks the player!");
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
    }

    private void Search()
    {
        // Optional: Add search pattern or last known position behavior
        Debug.Log("Searching for player...");
    }
}
