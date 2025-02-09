using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    [SerializeField] float moveSpeed = 1f;
    private GameObject target;
    SpriteRenderer sr;
    Rigidbody2D myRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        // Find the player object in the scene and assign it as the target
        target = GameObject.FindGameObjectWithTag("Player");

        // Check if the player object is found to avoid null reference errors
        if (target == null)
        {
            Debug.LogError("Player object not found! Make sure your player has the 'Player' tag.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector3 targetDirection = (target.transform.position - myRigidBody.transform.position).normalized;
        myRigidBody.velocity = targetDirection * moveSpeed * Time.deltaTime;
        if (gameObject.CompareTag("Enemy"))
        {
            FlipEnemySprite();
        }
    }

    private void FlipEnemySprite()
    {
        if (myRigidBody.velocity.x > 0)
        {
            sr.flipX = false; // Facing right
        }
        else if (myRigidBody.velocity.x < 0)
        {
            sr.flipX = true; // Facing left
        }
    }


    void FlipPlayer()
    {
        // Get the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Check if the mouse is left or right of the player

    }
}
