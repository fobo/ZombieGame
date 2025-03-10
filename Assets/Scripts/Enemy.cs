// using System;
// using UnityEngine;

// public class Enemy : MonoBehaviour, ISpawnable
// {
//     [SerializeField] public int damage = 10;
//     [SerializeField] private HealthComponent healthComponent;
//     [SerializeField] private GameObject itemPrefab; // Assign in Inspector
//     [SerializeField] private int treasureClassLevel = 1; // Set the enemy's loot level



//     private string poolKey;

//     private void Awake()
//     {
//         healthComponent = GetComponent<HealthComponent>();

//     }

//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         //static variables are stored in the BOOLS
//         if (collision.gameObject.CompareTag("Player"))
//         {
//             Damage damageC = new Damage(damage, false);
//             HealthComponent playerHealth = collision.gameObject.GetComponent<HealthComponent>();
//             if (playerHealth != null)
//             {
//                 playerHealth.TakeDamage(damageC);
//             }
//             else
//             {
//                 Debug.LogWarning("Player object does not have a HealthComponent!");
//             }
//         }
//     }

//     /// <summary>
//     /// Called when the enemy is spawned from the pool.
//     /// </summary>
//     public void OnSpawned(string poolKey)
//     {
//         this.poolKey = poolKey;
//     }

//     /// <summary>
//     /// Called when the enemy dies, returning it to the correct object pool.
//     /// </summary>
//     public void Die()
//     {
//         Debug.Log($"Returning {gameObject.name} to pool: {poolKey}");

//         //  Spawn a mystery item at the enemy's position
//         if (itemPrefab != null)
//         {
//             GameObject mysteryItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);

//             //  Assign the treasure class level to the spawned item
//             PrefabItemDropper prefrabItemScript = mysteryItem.GetComponent<PrefabItemDropper>();
//             if (prefrabItemScript != null)
//             {
//                 prefrabItemScript.SetTreasureClass(treasureClassLevel);
//             }
//         }
//         else
//         {
//             Debug.LogWarning($"Mystery item prefab is not assigned on {gameObject.name}!");
//         }

//         //  Reset and return enemy to pool
//         ResetEnemy();

//         if (!string.IsNullOrEmpty(poolKey))
//         {
//             GameController.Instance.ReturnToPool(poolKey, gameObject);
//         }
//         else
//         {
//             Debug.LogError($"Enemy {gameObject.name} has no poolKey assigned!");
//         }
//     }


//     private void ResetEnemy()
//     {
//         if (healthComponent != null)
//         {
//             healthComponent.ResetHealth();
//         }

//         Rigidbody2D rb = GetComponent<Rigidbody2D>();
//         if (rb != null)
//         {
//             rb.velocity = Vector2.zero;
//             rb.angularVelocity = 0f;
//         }
//     }
// }


using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public int damage = 10;
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private GameObject itemPrefab; // Assign in Inspector
    [SerializeField] private int minTC = 0; // Set the loot level of the chest
    [SerializeField] private int maxTC = 4;

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
        Debug.Log($"Enemy {gameObject.name} has died!");

        //  Spawn a mystery item at the enemy's position
        if (itemPrefab != null)
        {
            GameObject mysteryItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);

            //  Assign the treasure class level to the spawned item
            PrefabItemDropper prefrabItemScript = mysteryItem.GetComponent<PrefabItemDropper>();
            if (prefrabItemScript != null)
            {
                prefrabItemScript.SetTreasureClass(minTC, maxTC);
            }
        }
        else
        {
            Debug.LogWarning($"Mystery item prefab is not assigned on {gameObject.name}!");
        }

        Destroy(gameObject); // Destroy the enemy after it dies
    }
}
