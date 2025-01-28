using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public int damage = 10; // Amount of damage the enemy will deal to the player

private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        HealthComponent playerHealth = collision.gameObject.GetComponent<HealthComponent>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage, gameObject); // Pass this enemy as the damage source
        }
        else
        {
            Debug.LogWarning("Player object does not have a PlayerHealth component!");
        }
    }
}


    public void Die()
    {
        Debug.Log($"Enemy {gameObject.name} has been destroyed!");
        Destroy(gameObject);
    }
}
