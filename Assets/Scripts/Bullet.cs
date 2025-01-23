using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public float speed = 10f;
    [SerializeField] public float lifetime = 2f;
    [SerializeField] public int damage = 10;

    // Start is called before the first frame update
    void Start()
    {
        // Destroy the bullet after a certain lifetime
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        // Move the bullet to the left
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Bullet hit: " + other.name);

        // Check if the object has a HealthComponent
        HealthComponent healthComponent = other.GetComponent<HealthComponent>();

        if (healthComponent != null)
        {
            // Apply damage to the object's HealthComponent
            healthComponent.TakeDamage(damage);
        }
        Destroy(gameObject);


    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Bullet hit collider");
        
    }
}
