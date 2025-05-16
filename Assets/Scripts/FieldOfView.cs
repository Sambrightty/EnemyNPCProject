using UnityEngine;

public enum AwarenessState
{
    Unaware,
    Suspicious,
    Alerted,
    Engaged
}

public class FieldOfView : MonoBehaviour
{
    public float viewRadius = 10f;          // How far the enemy can see
    [Range(0, 360)]
    public float viewAngle = 120f;          // The angle of the FOV

    public LayerMask playerMask;            // Player layer
    public LayerMask obstacleMask;          // What blocks the view

    public Transform target;                // The player

    public bool CanSeePlayer { get; private set; }  // 👈 Add this property

    private AwarenessState currentState = AwarenessState.Unaware;
    private Renderer rend; // For color changes

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        CheckPlayerInSight();
    }

    void CheckPlayerInSight()
    {
        if (target == null) return;

        Vector3 dirToPlayer = (target.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2f)
        {
            if (!Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, obstacleMask))
            {
                // Player visible
                CanSeePlayer = true;
                Debug.DrawRay(transform.position, dirToPlayer * viewRadius, Color.green);

                if (distanceToPlayer < 3f)
                    SetAwarenessState(AwarenessState.Engaged);
                else
                    SetAwarenessState(AwarenessState.Alerted);
            }
            else
            {
                // Player blocked
                CanSeePlayer = false;
                Debug.DrawRay(transform.position, dirToPlayer * viewRadius, Color.red);
                SetAwarenessState(AwarenessState.Suspicious);
            }
        }
        else
        {
            // Player outside FOV
            CanSeePlayer = false;
            SetAwarenessState(AwarenessState.Unaware);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public void SetAwarenessState(AwarenessState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        Debug.Log("Awareness State changed to: " + newState);

        switch (currentState)
        {
            case AwarenessState.Unaware:
                rend.material.color = Color.gray;
                break;
            case AwarenessState.Suspicious:
                rend.material.color = Color.yellow;
                break;
            case AwarenessState.Alerted:
                rend.material.color = new Color(1f, 0.5f, 0f); // orange
                break;
            case AwarenessState.Engaged:
                rend.material.color = Color.red;
                break;
        }
    }

    public void HearPlayer()
    {
        SetAwarenessState(AwarenessState.Suspicious);
    }
}
