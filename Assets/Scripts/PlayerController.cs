using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Movement
    [SerializeField] private float moveSpeed = 1f;
    private Rigidbody2D myRigidBody;
    [SerializeField] private SpriteRenderer playerSprite; // Assign in Inspector
    private Animator playerAnimator;

    // Reference to the equipped gun
    [SerializeField] public Gun equippedGun;

    [SerializeField] private WeaponData[] weaponSlots; // Customize as needed
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    private bool canShootSingleFire = true; // Prevents rapid single-fire shots

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            EventBus.Instance?.PrintInventory();
        }
        //  Handle Single Fire & Shotgun (One shot per click)
        if (Input.GetMouseButtonDown(0))
        {
            if ((equippedGun.weaponData.fireType == FireType.SingleFire || equippedGun.weaponData.fireType == FireType.Shotgun) && canShootSingleFire)
            {
                //Debug.Log("Single fire shot");
                equippedGun.Shoot();
                canShootSingleFire = false; // Prevents continuous shooting until released
            }
        }

        //  Handle Automatic Fire (Hold to shoot)
        if (Input.GetMouseButton(0)) // Detects if the button is HELD
        {

            if (equippedGun.weaponData.fireType == FireType.Automatic)
            {
                //Debug.Log("Automatic fire shot");
                equippedGun.Shoot(); // Fire the gun (FireRateCooldown inside Shoot() handles timing)
            }
        }

        //  Reset Single-Fire on Button Release
        if (Input.GetMouseButtonUp(0))
        {
            canShootSingleFire = true;
        }

        //  Handle Weapon Switching
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SwitchWeapon(weaponSlots[i]);
            }
        }

        RotateTowardsMouse();
    }




    private void FixedUpdate()
    {
        //  Check if the player is moving
        bool isMoving = myRigidBody.velocity.magnitude > 0.1f;

        //  Set Animator parameter
        playerAnimator.SetBool("isMoving", isMoving);
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();

        if (context.canceled)
        {
            myRigidBody.velocity = Vector2.zero; //  Stop movement when no input
            return;
        }

        //  Normal movement
        Vector3 direction = new Vector3(inputVector.x, inputVector.y, 0).normalized;
        myRigidBody.velocity = direction * moveSpeed;
    }


    void SwitchWeapon(WeaponData newWeapon)
    {
        if (equippedGun != null)
        {
            equippedGun.EquipWeapon(newWeapon);
        }
    }




    void RotateTowardsMouse()
    {
        // Get the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Check if the mouse is left or right of the player
        playerSprite.flipX = mousePosition.x < transform.position.x;
    }


    public void Die()
    {
        Debug.Log($"Player {gameObject.name} has been destroyed!");
        Destroy(gameObject);
    }
}
