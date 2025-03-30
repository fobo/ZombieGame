using UnityEngine;
using System.Collections.Generic;

public class AutoPickupZone2D : MonoBehaviour
{
    [SerializeField] private float maxPullSpeed = 10f;
    [SerializeField] private float accelerationDuration = 0.5f; // Time to reach max speed
    [SerializeField] private string targetTag = "Floater";

    private class FloaterData
    {
        public Transform transform;
        public float timeInside;
    }

    private List<FloaterData> floatingObjects = new List<FloaterData>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            if (!floatingObjects.Exists(f => f.transform == other.transform))
            {
                floatingObjects.Add(new FloaterData { transform = other.transform, timeInside = 0f });
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            floatingObjects.RemoveAll(f => f.transform == other.transform);
        }
    }

    private void Update()
    {
        Vector3 center = transform.position;

        for (int i = floatingObjects.Count - 1; i >= 0; i--)
        {
            var floater = floatingObjects[i];

            if (floater.transform == null)
            {
                floatingObjects.RemoveAt(i);
                continue;
            }

            // Update time inside the field
            floater.timeInside += Time.deltaTime;

            // Ease-in speed based on time
            float t = Mathf.Clamp01(floater.timeInside / accelerationDuration);
            float currentSpeed = Mathf.Lerp(0f, maxPullSpeed, t);

            // Rotate toward center
            Vector3 direction = center - floater.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            floater.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Move toward center
            floater.transform.position = Vector3.MoveTowards(
                floater.transform.position, center, currentSpeed * Time.deltaTime);
        }
    }
}
