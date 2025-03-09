using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class PickupShaderApplier : MonoBehaviour
{
    public Material pickupMaterial; // Assign via Inspector

    private Material originalMaterial;
    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            originalMaterial = objectRenderer.material; // Store the original material

            if (pickupMaterial == null)
            {
                pickupMaterial = Resources.Load<Material>("PickupHighlightMaterial");
            }

            if (pickupMaterial != null)
            {
                objectRenderer.material = pickupMaterial;
            }
            else
            {
                Debug.LogError("Pickup Highlight Material not found! Make sure it's in a 'Resources' folder.");
            }
        }
    }

    void OnDestroy()
    {
        // Restore the original material when destroyed (optional)
        if (objectRenderer != null)
        {
            objectRenderer.material = originalMaterial;
        }
    }
}
