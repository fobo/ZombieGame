using System.Collections;
using UnityEngine;

public class DamageFlashController : MonoBehaviour
{
    [Header("Flash Settings")]
    [SerializeField] private Material flashMaterial;       // Reference to the flash material.
    [SerializeField] private float flashDuration = 0.1f;   // Duration of the flash.

    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalMaterial = spriteRenderer.material; // Store the original material.
        }
    }

    public void TriggerFlash()
    {
        if (spriteRenderer != null && flashMaterial != null)
        {
            StopAllCoroutines(); // Prevent multiple flashes from stacking.

            // Check if the gameObject is active before starting the coroutine
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(FlashCoroutine());
            }
            else
            {
                Debug.LogWarning($"{gameObject.name} is inactive, cannot start FlashCoroutine.");
            }
        }
    }


    private IEnumerator FlashCoroutine()
    {
        // Assign the flash material to the sprite.
        spriteRenderer.material = flashMaterial;

        // Set FlashAmount to maximum.
        flashMaterial.SetFloat("_FlashAmount", 1f);

        float elapsedTime = 0f;
        while (elapsedTime < flashDuration)
        {
            // Gradually reduce FlashAmount from 1 to 0 over the flashDuration.
            float flashValue = Mathf.Lerp(1f, 0f, elapsedTime / flashDuration);
            flashMaterial.SetFloat("_FlashAmount", flashValue);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure FlashAmount is set to 0 at the end.
        flashMaterial.SetFloat("_FlashAmount", 0f);

        // Restore the original material.
        spriteRenderer.material = originalMaterial;
    }
}
