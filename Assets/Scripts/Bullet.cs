using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;
    public float lifetime = 3f;

    void Start()
    {

        // Destroy the bullet after a certain lifetime (2 seconds)
        Destroy(gameObject, lifetime);
    }
    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HealthComponent health = other.GetComponent<HealthComponent>();
        if (health != null)
        {
            health.TakeDamage(damage, gameObject);
        }

        Destroy(gameObject); // Destroy the bullet after hitting something
    }
}
