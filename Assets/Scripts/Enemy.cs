using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public int damage = 10;
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private GameObject itemPrefab; // Assign in Inspector
    [SerializeField] private int TC = 0;
    [SerializeField] public uint hitID;

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Damage damageC = new Damage(damage, false, 0f);
            HealthComponent playerHealth = collision.gameObject.GetComponent<HealthComponent>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageC);
            }
            else
            {
                Debug.LogWarning("Player object does not have a HealthComponent!");
            }
        }
    }

    /// <summary>
    /// Called when the enemy dies.
    /// </summary>
    public void Die()
    {
       // Debug.Log($"Enemy {gameObject.name} has died!");

        //  Spawn a mystery item at the enemy's position
        if (itemPrefab != null)
        {
            GameObject mysteryItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);

            //  Assign the treasure class level to the spawned item
            PrefabItemDropper prefrabItemScript = mysteryItem.GetComponent<PrefabItemDropper>();
            if (prefrabItemScript != null)
            {
                prefrabItemScript.SetTreasureClass(TC);
            }
        }
        else
        {
            Debug.LogWarning($"Mystery item prefab is not assigned on {gameObject.name}!");
        }

        Destroy(gameObject); // Destroy the enemy after it dies
    }
}
