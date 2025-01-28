using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;
    public float lifetime = 2f;

        void Start()
    {

        // Destroy the bullet after a certain lifetime (2 seconds)
        Destroy(gameObject, lifetime);
    }
    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) // Adjust to 3D or 2D based on your game
    {
        HealthComponent health = other.GetComponent<HealthComponent>();
        if (health != null)
        {
            health.TakeDamage(damage, gameObject);
        }

        Destroy(gameObject); // Destroy the bullet after hitting something
    }
}
