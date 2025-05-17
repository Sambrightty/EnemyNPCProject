using UnityEngine;

public class EnemyGrudgeMemory : MonoBehaviour
{
    private string grudgeKey = "EnemyGrudgeLevel";
    public int grudgeLevel = 0;

    void Start()
    {
        // Load grudge from PlayerPrefs
        grudgeLevel = PlayerPrefs.GetInt(grudgeKey, 0);
        Debug.Log("Grudge Level Loaded: " + grudgeLevel);
    }

    public void IncreaseGrudge()
    {
        grudgeLevel++;
        PlayerPrefs.SetInt(grudgeKey, grudgeLevel);
        PlayerPrefs.Save();
        Debug.Log("Grudge Level Increased to: " + grudgeLevel);
    }

    public void ResetGrudge()
    {
        grudgeLevel = 0;
        PlayerPrefs.DeleteKey(grudgeKey);
        Debug.Log("Grudge Level Reset");
    }

    public int GetGrudgeLevel()
    {
        return grudgeLevel;
    }
}
