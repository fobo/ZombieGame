using TMPro;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public TextMeshProUGUI tmpText; 
    public float delay = 5f;        // Time before fade starts
    public float fadeDuration = 2f; // How long it takes to fade

    private void Start()
    {
        if (tmpText == null)
            tmpText = GetComponent<TextMeshProUGUI>();

        StartCoroutine(FadeOutAfterDelay());
    }

    private System.Collections.IEnumerator FadeOutAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        Color originalColor = tmpText.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            tmpText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }


        tmpText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}
