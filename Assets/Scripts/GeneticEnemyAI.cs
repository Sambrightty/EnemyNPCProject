using UnityEngine;
using TMPro; 

public class GeneticEnemyAI : MonoBehaviour
{
    [Header("Enemy Genes (Traits)")]
    [Range(0f, 1f)] public float aggression = 0.5f;
    [Range(0f, 1f)] public float defense = 0.5f;
    [Range(0f, 1f)] public float dodge = 0.5f;

    [Header("Genetic Algorithm Settings")]
    [Range(0f, 0.5f)] public float mutationRate = 0.1f;

    private const string saveKeyPrefix = "EnemyGene_";
    
    public TextMeshProUGUI aggressionText;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI dodgeText;

    private void Start()
  {
    LoadGenes();
  }

    public void MutateGenes()
    {
        aggression = MutateValue(aggression);
        defense = MutateValue(defense);
        dodge = MutateValue(dodge);

        SaveGenes();
        Debug.Log($"Genes mutated: Aggression={aggression:F2}, Defense={defense:F2}, Dodge={dodge:F2}");
    }

    private float MutateValue(float value)
    {
        if (Random.value < mutationRate)
        {
            float mutationAmount = Random.Range(-0.1f, 0.1f);
            value = Mathf.Clamp01(value + mutationAmount);
        }
        return value;
    }

    public void SaveGenes()
    {
        PlayerPrefs.SetFloat(saveKeyPrefix + "Aggression", aggression);
        PlayerPrefs.SetFloat(saveKeyPrefix + "Defense", defense);
        PlayerPrefs.SetFloat(saveKeyPrefix + "Dodge", dodge);
        PlayerPrefs.Save();
    }

    public void LoadGenes()
    {
        aggression = PlayerPrefs.GetFloat(saveKeyPrefix + "Aggression", aggression);
        defense = PlayerPrefs.GetFloat(saveKeyPrefix + "Defense", defense);
        dodge = PlayerPrefs.GetFloat(saveKeyPrefix + "Dodge", dodge);
    }

    public string DecideNextAction()
    {
        float roll = Random.value;

        if (roll < aggression) return "Attack";
        if (roll < aggression + defense) return "Block";
        if (roll < aggression + defense + dodge) return "Dodge";

        return "Idle";
    }

    public void EvolveGene(string behavior)
    {
        Debug.Log($"🧬 Evolving genes based on player behavior: {behavior}");

        switch (behavior)
        {
            case "Aggressive":
                defense = MutateValue(defense + 0.1f);
                dodge = MutateValue(dodge + 0.1f);
                break;

            case "Defensive":
                aggression = MutateValue(aggression + 0.1f);
                break;

            case "Balanced":
                MutateGenes();
                break;
        }

        SaveGenes();
    }
    
    private void Update()
    {
        if (aggressionText != null)
            aggressionText.text = $"Aggression: {aggression:F2}";
        if (defenseText != null)
            defenseText.text = $"Defense: {defense:F2}";
        if (dodgeText != null)
            dodgeText.text = $"Dodge: {dodge:F2}";
    }

  //   private void OnGUI()
  // {
  //   GUI.Label(new Rect(10, 10, 300, 80),
  //       $"Genes:\nAggression: {aggression:F2}\nDefense: {defense:F2}\nDodge: {dodge:F2}");
  // }
}
