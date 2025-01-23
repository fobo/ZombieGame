using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{


    //movement
    [SerializeField] float moveSpeed = 1f;
    
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


        
        if(Input.GetMouseButtonDown(0)){
            Shoot();
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

    void Shoot(){
        Instantiate(Bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
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
}
