using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100; // Maximum health.
    private int currentHealth;

    // Unity Event triggered when health reaches zero.
    public UnityEvent onHealthDepleted;

    // Unity Event triggered whenever health changes (optional).
    public UnityEvent<int, int> onHealthChanged;

    private void Awake()
    {
        // Initialize current health to the max health.
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Apply damage to the object.
    /// </summary>
    /// <param name="damageAmount">Amount of damage to apply.</param>
    public void TakeDamage(int damageAmount)
    {
        if (damageAmount <= 0 || currentHealth <= 0) return;

        currentHealth -= damageAmount;

        // Clamp health to ensure it doesn't drop below 0.
        currentHealth = Mathf.Max(currentHealth, 0);

        // Notify listeners about the health change.
        onHealthChanged?.Invoke(currentHealth, maxHealth);

        // If health reaches zero, invoke the health-depleted event.
        if (currentHealth == 0)
        {
            onHealthDepleted?.Invoke();
        }
    }

    /// <summary>
    /// Heal the object.
    /// </summary>
    /// <param name="healAmount">Amount of health to restore.</param>
    public void Heal(int healAmount)
    {
        if (healAmount <= 0 || currentHealth <= 0) return;

        currentHealth += healAmount;

        // Clamp health to ensure it doesn't exceed max health.
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        // Notify listeners about the health change.
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    /// <summary>
    /// Reset health to max health.
    /// </summary>
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    /// <summary>
    /// Get the current health value.
    /// </summary>
    public int GetCurrentHealth() => currentHealth;

    /// <summary>
    /// Get the maximum health value.
    /// </summary>
    public int GetMaxHealth() => maxHealth;
}
