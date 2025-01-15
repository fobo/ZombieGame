using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] public float speed = 10f;
    [SerializeField] public float lifetime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
        Destroy(gameObject, lifetime); //destroy me after life time expires
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("I just hit a " + other.name);

        // Destroy the parent of the object hit if it exists
        Transform parentTransform = other.transform.parent;
        if (parentTransform != null)
        {
            Destroy(parentTransform.gameObject); // Destroy the parent GameObject
        }
        else
        {
            Destroy(other.gameObject); // Destroy the collided GameObject itself
        }

        // Destroy the bullet after collision
        Destroy(gameObject);
    }
}
