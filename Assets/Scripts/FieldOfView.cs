// // using UnityEngine;

// // public enum AwarenessState
// // {
// //     Unaware,
// //     Suspicious,
// //     Alerted,
// //     Engaged
// // }


// // public class FieldOfView : MonoBehaviour
// // {
// //     public float viewRadius = 10f;          // How far the enemy can see
// //     [Range(0, 360)]
// //     public float viewAngle = 120f;          // The angle of the FOV

// //     public LayerMask playerMask;            // Player layer
// //     public LayerMask obstacleMask;          // What blocks the view

// //     public Transform target;                // The player

// //     private AwarenessState currentState = AwarenessState.Unaware;
// //     private Renderer rend; // For color changes

// //     void Start()
// //     {
// //         rend = GetComponent<Renderer>();
// //     }

// //     void Update()
// //     {
// //         CheckPlayerInSight();
// //     }

// //     // Was first used to test FOV
// //     // void CheckPlayerInSight()
// //     // {
// //     //     if (target == null) return;

// //     //     // Direction to the player
// //     //     Vector3 dirToPlayer = (target.position - transform.position).normalized;

// //     //     // Distance to player
// //     //     float distanceToPlayer = Vector3.Distance(transform.position, target.position);

// //     //     // Check if within view angle
// //     //     if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2f)
// //     //     {
// //     //         // Raycast to check for obstacles
// //     //         if (!Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, obstacleMask))
// //     //         {
// //     //             Debug.Log("Player in Sight!");
// //     //             Debug.DrawRay(transform.position, dirToPlayer * viewRadius, Color.green);
// //     //         }
// //     //         else
// //     //         {
// //     //             Debug.Log("Player Blocked by Obstacle");
// //     //             Debug.DrawRay(transform.position, dirToPlayer * viewRadius, Color.red);
// //     //         }
// //     //     }
// //     //     else
// //     //     {
// //     //         Debug.Log("Player Outside FOV");
// //     //     }
// //     // }

// //     // // Helper method to convert angle to direction
// //     // public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
// //     // {
// //     //     if (!angleIsGlobal)
// //     //         angleInDegrees += transform.eulerAngles.y;

// //     //     return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
// //     // }

// //     void CheckPlayerInSight()
// //     {
// //         if (target == null) return;

// //         Vector3 dirToPlayer = (target.position - transform.position).normalized;
// //         float distanceToPlayer = Vector3.Distance(transform.position, target.position);

// //         if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2f)
// //         {
// //             if (!Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, obstacleMask))
// //             {
// //                 // Player visible
// //                 Debug.DrawRay(transform.position, dirToPlayer * viewRadius, Color.green);

// //                 if (distanceToPlayer < 3f)
// //                     SetAwarenessState(AwarenessState.Engaged);
// //                 else
// //                     SetAwarenessState(AwarenessState.Alerted);
// //             }
// //             else
// //             {
// //                 // Player blocked
// //                 Debug.DrawRay(transform.position, dirToPlayer * viewRadius, Color.red);
// //                 SetAwarenessState(AwarenessState.Suspicious);
// //             }
// //         }
// //         else
// //         {
// //             // Player outside FOV
// //             SetAwarenessState(AwarenessState.Unaware);
// //         }
// //     }

// //     public void SetAwarenessState(AwarenessState newState)
// //     {
// //         if (currentState == newState) return;

// //         currentState = newState;
// //         Debug.Log("Awareness State changed to: " + newState);

// //         switch (currentState)
// //         {
// //             case AwarenessState.Unaware:
// //                 rend.material.color = Color.gray;
// //                 break;
// //             case AwarenessState.Suspicious:
// //                 rend.material.color = Color.yellow;
// //                 break;
// //             case AwarenessState.Alerted:
// //                 rend.material.color = new Color(1f, 0.5f, 0f); // orange
// //                 break;
// //             case AwarenessState.Engaged:
// //                 rend.material.color = Color.red;
// //                 break;
// //         }
// //     }

// //     public void HearPlayer()
// //     {
// //         SetAwarenessState(AwarenessState.Suspicious);
// //     }



// // }


// using UnityEngine;

// public enum AwarenessState
// {
//     Unaware,
//     Suspicious,
//     Alerted,
//     Engaged
// }

// public class FieldOfView : MonoBehaviour
// {
//     public float viewRadius = 10f;          // How far the enemy can see
//     [Range(0, 360)]
//     public float viewAngle = 120f;          // The angle of the FOV

//     public float hearingRadius = 5f;        // How close the player must be to be heard

//     public LayerMask playerMask;            // Player layer
//     public LayerMask obstacleMask;          // What blocks the view

//     public Transform target;                // The player

//     private AwarenessState currentState = AwarenessState.Unaware;
//     private Renderer rend;                  // For color changes

//     void Start()
//     {
//         rend = GetComponent<Renderer>();
//     }

//     void Update()
//     {
//         CheckPlayerInSight();
//         CheckHearing(); // Call hearing check every frame
//     }

//     void CheckPlayerInSight()
//     {
//         if (target == null) return;

//         Vector3 dirToPlayer = (target.position - transform.position).normalized;
//         float distanceToPlayer = Vector3.Distance(transform.position, target.position);

//         if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2f)
//         {
//             if (!Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, obstacleMask))
//             {
//                 // Player visible
//                 Debug.DrawRay(transform.position, dirToPlayer * viewRadius, Color.green);

//                 if (distanceToPlayer < 3f)
//                     SetAwarenessState(AwarenessState.Engaged);
//                 else
//                     SetAwarenessState(AwarenessState.Alerted);
//             }
//             else
//             {
//                 // Player blocked
//                 Debug.DrawRay(transform.position, dirToPlayer * viewRadius, Color.red);
//                 SetAwarenessState(AwarenessState.Suspicious);
//             }
//         }
//         else
//         {
//             // Player outside FOV
//             SetAwarenessState(AwarenessState.Unaware);
//         }
//     }

//     void CheckHearing()
//     {
//         if (target == null) return;

//         float distance = Vector3.Distance(transform.position, target.position);
//         if (distance <= hearingRadius && currentState == AwarenessState.Unaware)
//         {
//             Debug.Log("Sound detected! Switching to Suspicious state.");
//             HearPlayer();
//         }
//     }

//     public void HearPlayer()
//     {
//         SetAwarenessState(AwarenessState.Suspicious);
//     }

//     public void SetAwarenessState(AwarenessState newState)
//     {
//         if (currentState == newState) return;

//         currentState = newState;
//         Debug.Log("Awareness State changed to: " + newState);

//         switch (currentState)
//         {
//             case AwarenessState.Unaware:
//                 rend.material.color = Color.gray;
//                 break;
//             case AwarenessState.Suspicious:
//                 rend.material.color = Color.yellow;
//                 break;
//             case AwarenessState.Alerted:
//                 rend.material.color = new Color(1f, 0.5f, 0f); // orange
//                 break;
//             case AwarenessState.Engaged:
//                 rend.material.color = Color.red;
//                 break;
//         }
//     }
// }

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
    public float viewRadius = 10f;
    [Range(0, 360)]
    public float viewAngle = 120f;
    public float hearingRadius = 5f;

    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public Transform target;

    private AwarenessState currentState = AwarenessState.Unaware;
    private Renderer rend;

    // ✅ This is the missing field
    public bool canSeePlayer { get; private set; }

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        CheckPlayerInSight();
        CheckHearing();
    }

    void CheckPlayerInSight()
    {
        if (target == null)
        {
            canSeePlayer = false;
            return;
        }

        Vector3 dirToPlayer = (target.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2f)
        {
            if (!Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, obstacleMask))
            {
                // Player visible
                Debug.DrawRay(transform.position, dirToPlayer * viewRadius, Color.green);
                canSeePlayer = true;

                if (distanceToPlayer < 3f)
                    SetAwarenessState(AwarenessState.Engaged);
                else
                    SetAwarenessState(AwarenessState.Alerted);
                return;
            }
        }

        // Player not visible
        canSeePlayer = false;

        if (distanceToPlayer <= hearingRadius && currentState == AwarenessState.Unaware)
        {
            SetAwarenessState(AwarenessState.Suspicious);
        }
        else
        {
            SetAwarenessState(AwarenessState.Unaware);
        }
    }

    void CheckHearing()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= hearingRadius && currentState == AwarenessState.Unaware)
        {
            Debug.Log("Sound detected! Switching to Suspicious state.");
            HearPlayer();
        }
    }

    public void HearPlayer()
    {
        SetAwarenessState(AwarenessState.Suspicious);
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
}
