using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Movement
    [SerializeField] private float moveSpeed = 1f;
    private Rigidbody2D myRigidBody;
    [SerializeField] private SpriteRenderer playerSprite; // Assign in Inspector
    private Animator playerAnimator;
    public GameObject deathMenu;
    public GameObject popupTextField; // used for pickups, reload status, etc
    public Transform textSpawnPoint; // position where text spawns from the player.
    public HealthComponent hc; // reference to the health component
    public Vector2 moveDir = Vector2.zero;


    // Reference to the equipped gun
    [SerializeField] public Gun equippedGun;

    [SerializeField] private WeaponData[] weaponSlots; // Customize as needed

    [SerializeField] public GameObject turret; //placeholder under we figure out how we want to add more turrets



    //events


    private void Awake()
    {

        // Ensure only one player exists
        if (FindObjectsOfType<PlayerController>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // Keep player across levels
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(WaitForEventBus());
        if (EventBus.Instance != null)
        {
            EventBus.Instance.OnMomentoPickedUp += UpdatePlayerStats;
        }
        else
        {
            //Debug.LogError("EventBus is not initialized yet! Delaying subscription.");
            //StartCoroutine(WaitForEventBus());
        }
    }

    private IEnumerator WaitForEventBus()
    {
        while (EventBus.Instance == null)
        {
            yield return null; // Wait until EventBus is initialized
        }

        // Now that EventBus exists, subscribe to the event
        EventBus.Instance.OnMomentoPickedUp += UpdatePlayerStats;
//        Debug.Log("Successfully subscribed to OnMomentoPickedUp.");
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (EventBus.Instance != null)
        {
            EventBus.Instance.OnMomentoPickedUp -= UpdatePlayerStats;
        }
    }



    private void UpdatePlayerStats()
    {
        hc.SetMaxHealth((int)(hc.GetOriginalMaxHealth() + MomentoSystem.Instance.GetHealthMultiplier()));
        hc.Heal((int)hc.GetMaxHealth()); //player found a health upgrade, might as well heal them to full as well.
        hc.UpdateUI();
//        Debug.Log("Current max health: " + hc.GetMaxHealth());
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MoveToSpawnPoint();
    }

    public void MoveToSpawnPoint()
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position; // Move player
        }
        else
        {
            Debug.LogWarning("No SpawnPoint found in the scene!");
        }
    }


    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        hc = GetComponent<HealthComponent>();
        deathMenu.SetActive(false); // make the death menu not appear yet
    }

    private bool canShootSingleFire = true; // Prevents rapid single-fire shots

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && InventorySystem.Instance.GetAmmoCount(AmmoType.defaultTurret) > 0)
        { // check if player is pressing deploy key and has a turret
            InventorySystem.Instance.UseAmmo(AmmoType.defaultTurret, 1); //uses one turret
            Instantiate(turret, transform.position, Quaternion.identity);

            //Deploy turret at the players feet
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            //EventBus.Instance?.PrintInventory();
        }
        //  Handle Single Fire & Shotgun (One shot per click)
        if (equippedGun.weaponData != null && Input.GetMouseButtonDown(0))
        {
            if ((equippedGun.weaponData.fireType == FireType.SingleFire || equippedGun.weaponData.fireType == FireType.Shotgun) && canShootSingleFire)
            {
                //Debug.Log("Single fire shot");
                equippedGun.Shoot();
                canShootSingleFire = false; // Prevents continuous shooting until released
            }
        }

        //  Handle Automatic Fire (Hold to shoot)
        if (equippedGun.weaponData != null && Input.GetMouseButton(0)) // Detects if the button is HELD
        {

            if (equippedGun.weaponData.fireType == FireType.Automatic || equippedGun.weaponData.fireType == FireType.Melee)
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
                if (InventorySystem.Instance.HasWeapon(weaponSlots[i].weaponName)) //  Check if player owns weapon
                {
                    SwitchWeapon(weaponSlots[i]);
                }
                else
                {
                    Debug.LogWarning($"You do not own {weaponSlots[i].weaponName}!");
                }
            }
        }

        FlipPlayer();
        RotateGunAroundPlayer();
    }




    private void FixedUpdate()
    {
        //  Check if the player is moving
        bool isMoving = myRigidBody.velocity.magnitude > 0.1f;

        //  Set Animator parameter
        playerAnimator.SetBool("isMoving", isMoving);
        myRigidBody.velocity = moveDir * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>().normalized;
        Vector2 inputVector = context.ReadValue<Vector2>();


    }


    public void SwitchWeapon(WeaponData newWeapon)
    {
        if (equippedGun != null)
        {
            equippedGun.EquipWeapon(newWeapon);
        }
    }




    void FlipPlayer()
    {
        // Get the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Check if the mouse is left or right of the player
        playerSprite.flipX = mousePosition.x < transform.position.x;
    }

    [SerializeField] private float gunDistance = 0.75f; // Adjust this in the Inspector

    void RotateGunAroundPlayer()
    {
        if (equippedGun == null) return;

        // Get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure it's in 2D space

        // Get direction from player to mouse
        Vector3 direction = (mousePosition - transform.position).normalized;

        //  Move gun further from player by increasing the distance
        Vector3 gunPosition = transform.position + direction * gunDistance;

        // Move gun to calculated position
        equippedGun.transform.position = gunPosition;

        // Rotate gun to face the mouse cursor
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        equippedGun.transform.rotation = Quaternion.Euler(0, 0, angle);
    }


    public void SpawnTextPopup(Text text)
    {
        if (popupTextField == null) { return; } //null check
        Vector3 spawnPos = textSpawnPoint.position;
        GameObject popupTextInstance = Instantiate(popupTextField, spawnPos, Quaternion.identity);

        //get the script component, "PopupText"
        PopupText popupTextScript = popupTextInstance.GetComponent<PopupText>();
        //if it is not null, assign the textMeshProGUI to "text"
        if (popupTextScript != null)
        {
            popupTextScript.textMeshProUGUI.SetText(text.text);
            popupTextScript.textMeshProUGUI.color = text.color;
        }
        else
        {
            Debug.LogWarning("PopupText component not found on instantiated object!");
        }

    }



    public void Die()
    {
        deathMenu.SetActive(true);
        Debug.Log($"Player {gameObject.name} has been destroyed!");
        Destroy(gameObject);
    }
}
