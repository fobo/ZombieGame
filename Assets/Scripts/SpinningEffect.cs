using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningEffect : MonoBehaviour
{
    public float rotationSpeed = 10f;

    // Pulsating effect variables
    public float pulseSpeed = 2f;       // Speed of pulsation
    public float pulseScale = 0.1f;     // Amount to scale (e.g., 0.1 = ±10%)
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void FixedUpdate()
    {
        // Rotate around Y axis
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // Pulsate (scale in and out over time)
        float scaleFactor = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseScale;
        transform.localScale = originalScale * scaleFactor;
    }
}
