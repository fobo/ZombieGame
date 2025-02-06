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

    private string[] weaponSlots = { "AK47", "Shotgun" }; // Customize as needed
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    private bool canShootSingleFire = true; // Prevents rapid single-fire shots

    void Update()
    {
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

    private void SwitchWeapon(string weaponName)
    {
        if (InventorySystem.Instance.HasWeapon(weaponName))
        {
            WeaponData newWeapon = InventorySystem.Instance.GetWeaponData(weaponName);
            if (newWeapon != null)
            {
                equippedGun.EquipWeapon(newWeapon);
            }
        }
        else
        {
            Debug.Log($"You don't have {weaponName} yet!");
        }
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
