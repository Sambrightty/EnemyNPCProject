using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack,
        Search,
        Retreat
    }

    public EnemyState currentState = EnemyState.Patrol;

    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetState(EnemyState.Patrol); // Start in Patrol
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                Debug.Log("🔵 Patrol State");
                Patrol();
                break;
            case EnemyState.Chase:
                Debug.Log("🔴 Chase State");
                break;
            case EnemyState.Attack:
                Debug.Log("🟡 Attack State");
                break;
            case EnemyState.Search:
                Debug.Log("🟠 Search State");
                break;
            case EnemyState.Retreat:
                Debug.Log("⚫ Retreat State");
                break;
        }
    }

    public void SetState(EnemyState newState)
    {
        if (currentState != newState)
        {
            Debug.Log($"State changed from {currentState} to {newState}");
            currentState = newState;
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0 || agent == null) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }
}
