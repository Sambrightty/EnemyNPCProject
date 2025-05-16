using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public KeyCode punchKey = KeyCode.Space;
    public KeyCode blockKey = KeyCode.B;

    private Rigidbody rb;
    private bool isBlocking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();

        if (Input.GetKeyDown(punchKey))
        {
            Punch();
        }

        if (Input.GetKeyDown(blockKey))
        {
            isBlocking = true;
            Debug.Log("Player is blocking.");
        }

        if (Input.GetKeyUp(blockKey))
        {
            isBlocking = false;
            Debug.Log("Player stopped blocking.");
        }
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float v = Input.GetAxis("Vertical");   // W/S or Up/Down

        Vector3 direction = new Vector3(h, 0, v).normalized;
        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
    }

    void Punch()
    {
        Debug.Log("Player punches!");
        // Later: Trigger animation or hitbox
    }

    public bool IsBlocking()
    {
        return isBlocking;
    }
}
