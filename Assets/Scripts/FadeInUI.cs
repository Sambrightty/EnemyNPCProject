using UnityEngine;

public class FadeInUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeSpeed = 1f;

    void OnEnable()
    {
        canvasGroup.alpha = 0f;
        StartCoroutine(FadeIn());
    }

    private System.Collections.IEnumerator FadeIn()
    {
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.unscaledDeltaTime * fadeSpeed;
            yield return null;
        }
    }
}
