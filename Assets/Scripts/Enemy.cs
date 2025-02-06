using System;
using UnityEngine;

public class Enemy : MonoBehaviour, ISpawnable
{
    [SerializeField] public int damage = 10;
    [SerializeField] private HealthComponent healthComponent;
    private string poolKey;

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthComponent playerHealth = collision.gameObject.GetComponent<HealthComponent>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage, gameObject);
            }
            else
            {
                Debug.LogWarning("Player object does not have a HealthComponent!");
            }
        }
    }

    /// <summary>
    /// Called when the enemy is spawned from the pool.
    /// </summary>
    public void OnSpawned(string poolKey)
    {
        this.poolKey = poolKey;
    }

    /// <summary>
    /// Called when the enemy dies, returning it to the correct object pool.
    /// </summary>
    public void Die()
    {
        Debug.Log($"Returning {gameObject.name} to pool: {poolKey}");
        ResetEnemy();

        if (!string.IsNullOrEmpty(poolKey))
        {
            GameController.Instance.ReturnToPool(poolKey, gameObject);
        }
        else
        {
            Debug.LogError($"Enemy {gameObject.name} has no poolKey assigned!");
        }
    }

    private void ResetEnemy()
    {
        if (healthComponent != null)
        {
            healthComponent.ResetHealth();
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}
