using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Movement
    [SerializeField] private float moveSpeed = 1f;
    private Rigidbody2D myRigidBody;

    // Reference to the equipped gun
    [SerializeField] private Gun equippedGun;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Handle shooting
        if (Input.GetMouseButton(0))
        {
            equippedGun.Shoot(); // Delegate shooting to the Gun script
        }

        RotateTowardsMouse();
    }

    private void FixedUpdate()
    {
        // Movement logic is called from the Input System
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 vector2 = context.ReadValue<Vector2>();

        // Move and slide logic
        Vector3 direction = new Vector3(vector2.x, vector2.y, 0).normalized;

        Vector3 moveVelocity = direction * moveSpeed * Time.deltaTime;

        myRigidBody.velocity = moveVelocity;
    }

    void RotateTowardsMouse()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Set z to 0 because we are in 2D

        // Calculate the direction from the player to the mouse
        Vector3 direction = mousePosition - transform.position;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation to the player
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void Die()
    {
        Debug.Log($"Player {gameObject.name} has been destroyed!");
        Destroy(gameObject);
    }
}
