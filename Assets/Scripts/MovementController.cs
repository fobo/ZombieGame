using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    [SerializeField] float moveSpeed = 1f;
    private GameObject target;

    Rigidbody2D myRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();


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
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z); // Face right
        }
        else if (myRigidBody.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z); // Face left
        }

    }
}
