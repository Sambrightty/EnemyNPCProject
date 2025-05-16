using UnityEngine;

public class PlayerBehaviorTracker : MonoBehaviour
{
  public int punchCount = 0;
  public int blockCount = 0;

  private float punchCooldown = 1f;
  private float blockCooldown = 1f;
  private float punchTimer = 0f;
  private float blockTimer = 0f;

  void Update()
  {
    punchTimer += Time.deltaTime;
    blockTimer += Time.deltaTime;

    if (Input.GetKeyDown(KeyCode.Space) && punchTimer > punchCooldown)
    {
      punchCount++;
      punchTimer = 0f;
      Debug.Log("Punch count: " + punchCount);
    }

    if (Input.GetKeyDown(KeyCode.B) && blockTimer > blockCooldown)
    {
      blockCount++;
      blockTimer = 0f;
      Debug.Log("Block count: " + blockCount);
    }
  }

  public string GetBehaviorType()
  {
    if (punchCount > blockCount + 3)
      return "Aggressive";
    else if (blockCount > punchCount + 3)
      return "Defensive";
    else
      return "Balanced";
  }

  public void ResetBehavior()
  {
    punchCount = 0;
    blockCount = 0;
  }
    
    // To show behavior type on screen temporarily:
    // void OnGUI()
  // {
  //     GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
  //     labelStyle.fontSize = 24;                      // 👈 Make the font larger
  //     labelStyle.normal.textColor = Color.white;     // 👈 Set color (optional)
  //     labelStyle.fontStyle = FontStyle.Bold;         // 👈 Optional: Bold text

  //     GUI.Label(new Rect(10, 10, 300, 30), "Behavior: " + GetBehaviorType(), labelStyle);
  // }


}
