using UnityEngine;

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

    void Start()
    {
        SetState(EnemyState.Chase);
    }


    // private float timer;
    // private float changeInterval = 5f;


    private void Update()
    {
      // timer += Time.deltaTime;

      // if (timer >= changeInterval)
      // {
      //     CycleState();
      //     timer = 0f;
      // }
        switch (currentState)
        {
            case EnemyState.Patrol:
                Debug.Log("🔵 Patrol State");
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

    
    void CycleState()
    {
        int next = ((int)currentState + 1) % System.Enum.GetValues(typeof(EnemyState)).Length;
        SetState((EnemyState)next);
    }

    public void SetState(EnemyState newState)
    {
        if (currentState != newState)
        {
            Debug.Log($"State changed from {currentState} to {newState}");
            currentState = newState;
        }
    }
}
