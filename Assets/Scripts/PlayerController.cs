using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{


    //movement
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] private float fireRate = 0.2f; // Time between shots
    
    [SerializeField] float spread = 5f; // Adjust this value to control the spread
    private float nextFireTime = 0f;

    public GameObject Bullet;
    public Transform bulletSpawnPoint;

    public float radius = 1f;             // Distance of the spawn point from the player
    public float rotationOffset = 90f;

    Rigidbody2D myRigidBody;


    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {


        
    if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
    {
        Shoot();
        nextFireTime = Time.time + fireRate;
    }

        RotateTowardsMouse();
    }

    private void FixedUpdate() {
        //Move();
    }


    public void Move(InputAction.CallbackContext context){

        Vector2 vector2 = context.ReadValue<Vector2>();

        //change to move and slide
        
        Vector3 direction = new Vector3(vector2.x, vector2.y, 0).normalized;

        Vector3 moveVelocity = direction * moveSpeed * Time.deltaTime;

        myRigidBody.velocity = moveVelocity;

    }

void Shoot()
{

    // Calculate a random spread angle
    float randomAngle = Random.Range(-spread, spread);

    // Apply the random spread to the bullet's rotation
    Quaternion spreadRotation = Quaternion.Euler(0, 0, bulletSpawnPoint.rotation.eulerAngles.z + randomAngle);

    // Instantiate the bullet with the modified rotation
    Instantiate(Bullet, bulletSpawnPoint.position, spreadRotation);
}


    void RotateTowardsMouse(){
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
        Debug.Log($"Enemy {gameObject.name} has been destroyed!");
        Destroy(gameObject);
    }
}
