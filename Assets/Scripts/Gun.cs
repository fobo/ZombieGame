using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Weapon Data")]
    public WeaponData weaponData; // Reference to the weapon's data
    [Header("Bullet Settings")]
    public Transform bulletSpawnPoint; // The point where bullets spawn
    public GameObject bulletPrefab;    // The bullet prefab (if using physical bullets)
    public GameObject rocketPrefab;
    public GameObject meleeAttackPrefab;


    public Transform magazineSpawnPoint; // Where the magazine spawns (e.g., under the gun)

    public Transform casingSpawnPoint; // where the casing will spawn (like a minigun will be more to the left, etc)

    public Animator gunAnimator; // links up to the animation player
    private int currentAmmo;
    private PlayerController player;

    [Header("Runtime Variables")]
    private bool isReloading;
    private bool canShoot = true;



    // BUILT IN METHODS
    private void Start()
    {

        if (gunAnimator == null)
        {
            gunAnimator = GetComponentInChildren<Animator>();
        }

        player = FindObjectOfType<PlayerController>();

    }

    void Update()
    {
        // Check if the player presses "R" and the gun is not already reloading, and if the player has a weapon equipped.
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && weaponData != null)
        {
            StartCoroutine(Reload());
        }
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForEventBusAndSubscribe());
    }

    private void OnDisable()
    {
        EventBus.Instance.OnMomentoPickedUp -= UpdateWeaponStats;
    }


    /////////////// events
    public delegate void ReloadAction(float reloadTime);
    public event ReloadAction OnReloadStart;


    private IEnumerator WaitForEventBusAndSubscribe()
    {
        while (EventBus.Instance == null)
        {
            yield return null; // Wait until EventBus is initialized
        }

        EventBus.Instance.OnMomentoPickedUp += UpdateWeaponStats;
        //        Debug.Log("Gun successfully subscribed to OnMomentoPickedUp.");
    }
    //


    public void EquipWeapon(WeaponData newWeaponData)
    {
        // Save current weapon's magazine ammo before switching
        if (weaponData != null)
        {
            InventorySystem.Instance.SaveWeaponMagazineAmmo(weaponData.weaponName, currentAmmo);
        }
        Debug.LogWarning("base bullet damage " + newWeaponData.baseDamage);
        Debug.LogWarning("bullet damage " + newWeaponData.damage);
        // Instantiate a fresh copy of the weapon data
        weaponData = ScriptableObject.Instantiate(newWeaponData);

        // Reset stats before applying multipliers (Prevents stacking!)
        ResetWeaponStats(weaponData);

        // Apply multipliers
        ApplyMomentoMultipliers(weaponData);

        // Retrieve the saved ammo count (or use max ammo if new weapon)
        currentAmmo = InventorySystem.Instance.GetSavedWeaponAmmo(weaponData.weaponName, weaponData.maxAmmo);

        // Update the HUD with the correct ammo
        EventBus.Instance?.UpdateGunAnimationUI();
        EventBus.Instance?.UpdateAmmoUI(currentAmmo, weaponData.maxAmmo);
    }





    public void Shoot()
    {
        if (currentAmmo == 0 && !isReloading && canShoot && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Text text = new Text("Reloading!", Color.red);
            StartCoroutine(Reload());
            player.SpawnTextPopup(text);
        }
        if (isReloading || !canShoot || currentAmmo <= 0) return;

        if (weaponData.fireType == FireType.Melee)
        {
            MeleeAttack();
            StartCoroutine(FireRateCooldown());
            return;
        }

        currentAmmo--;

        int bulletsPerShot = (weaponData.fireType == FireType.Shotgun) ? weaponData.bulletsPerShot : 1;

        FireWeapon(bulletsPerShot);


        StartCoroutine(FireRateCooldown());

        // Update HUD
        EventBus.Instance?.UpdateAmmoUI(currentAmmo, weaponData.maxAmmo);
    }










    private void FireWeapon(int bulletsPerShot)
    {
        for (int i = 0; i < bulletsPerShot; i++)
        {
            // Random spread angle in degrees
            float spreadAngle = Random.Range(-weaponData.spread, weaponData.spread);

            // Calculate the new bullet rotation based on the gun's current rotation
            Quaternion spreadRotation = Quaternion.Euler(0, 0, spreadAngle);

            // Determine the final firing direction
            Vector2 finalDirection = spreadRotation * transform.right;

            if (bulletSpawnPoint != null)
            {
                //spawn rocket if its a rocket
                if (weaponData.ammoType == AmmoType.Rocket)
                {
                    GameObject rocket = Instantiate(rocketPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation * spreadRotation);
                    

                    if (rocket != null)
                    {
                        Rocket bs = rocket.GetComponent<Rocket>();
                        bs.SetWeaponData(weaponData); // apply the current weapon data to the bullet

                        Rigidbody2D rb = rocket.GetComponent<Rigidbody2D>();
                        if (rb != null)
                        {
                            rb.velocity = finalDirection * .1f; // Set rocket speed
                        }
                    }

                    return;
                }


                // Get a bullet from the pool instead of Instantiating
                GameObject bullet = GameController.Instance.GetPooledObject("Bullet", bulletSpawnPoint.position, bulletSpawnPoint.rotation * spreadRotation);

                if (bullet != null)
                {
                    Bullet bs = bullet.GetComponent<Bullet>();
                    bs.SetWeaponData(weaponData); // apply the current weapon data to the bullet

                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.velocity = finalDirection * 20f; // Set bullet speed
                    }
                }
            }
        }

        EjectShellCasing(); // Eject shell casing (only one per shot)
    }





    public IEnumerator Reload()
    {
        if (isReloading) yield break; // you cant reload if you are already reloading.

        AmmoType ammoType = weaponData.ammoType;
        int availableAmmo = InventorySystem.Instance.GetAmmoCount(ammoType);
        int missingAmmo = weaponData.maxAmmo - currentAmmo;


        if (missingAmmo == 0 || availableAmmo <= 0)
        { // if the player has a full magazine, or has no ammo in the inventory, stop reload.
            Debug.Log("Player does not need to reload, or magazine is full.");
            yield break;
        }

        isReloading = true;
        Debug.Log($"Reloading {weaponData.weaponName}...");
        OnReloadStart?.Invoke(weaponData.reloadSpeed);
        //  Get the correct ammo type for this weapon


        //  Calculate how much ammo we need to refill



        //  Determine how much we can actually reload
        int ammoToReload = Mathf.Min(missingAmmo, availableAmmo);

        switch (weaponData.reloadType)
        {
            case ReloadType.Magazine:
                if (ammoToReload > 0)
                {
                    //  Drop empty magazine halfway through reload animation
                    Invoke("DropMagazine", weaponData.reloadSpeed / 2);
                    yield return new WaitForSeconds(weaponData.reloadSpeed); // Simulate reload time

                    //  Take ammo from inventory and put into the magazine
                    //InventorySystem.Instance.UseAmmo(ammoType, ammoToReload);
                    EventBus.Instance.UseAmmo(ammoType, ammoToReload);
                    currentAmmo += ammoToReload;
                }
                else
                {
                    Debug.Log("No ammo available to reload!");
                }
                break;

            case ReloadType.SingleShot:
            case ReloadType.Tube:
                if (ammoToReload > 0)
                {
                    yield return new WaitForSeconds(weaponData.reloadSpeed); // Simulate reload time

                    //  Take ammo from inventory and put into the magazine
                    //InventorySystem.Instance.UseAmmo(ammoType, ammoToReload);
                    EventBus.Instance.UseAmmo(ammoType, ammoToReload);
                    currentAmmo += ammoToReload;
                }
                else
                {
                    Debug.Log("No ammo available to reload!");
                }
                break;
        }

        isReloading = false;
        Debug.Log("Reload complete!");

        //  Final HUD update after reloading
        //HUDController.Instance?.UpdateAmmoUI(currentAmmo, weaponData.maxAmmo);
        EventBus.Instance?.UpdateAmmoUI(currentAmmo, weaponData.maxAmmo);
    }


    private IEnumerator FireRateCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(weaponData.fireRate * MomentoSystem.Instance.GetFireRateMultiplier()); // increases "fire rate"
        canShoot = true;
    }

    private void DropMagazine()
    {
        if (magazineSpawnPoint != null)
        {
            // Instantiate the magazine at the spawn point
            GameObject magazine = Instantiate(weaponData.magazineType, magazineSpawnPoint.position, Quaternion.identity);

            // Start the scaling and spinning effect
            StartCoroutine(AnimateMagazineDrop(magazine));
        }
    }


    private void EjectShellCasing()
    {
        if (casingSpawnPoint != null)
        {
            GameObject casing = Instantiate(weaponData.casingType, casingSpawnPoint.position, Quaternion.identity);
            casing.GetComponent<CasingBehavior>().Eject(transform.eulerAngles.z);
        }
    }



    private IEnumerator AnimateMagazineDrop(GameObject magazine)
    {
        float duration = 0.2f; // Total time for the animation
        float elapsedTime = 0f;

        // Initial and target scales
        Vector3 initialScale = magazine.transform.localScale * 1.5f; // 150% size
        Vector3 targetScale = magazine.transform.localScale;         // 100% size

        // Random spin angle (30 to 60 degrees)
        float spinAngle = Random.Range(5f, 355f);
        float spinDirection = Random.value > 0.5f ? 1f : -1f; // Randomize clockwise/counterclockwise spin

        // Animate scaling and spinning over time
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Interpolate scale
            float t = elapsedTime / duration;
            magazine.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            // Interpolate rotation 
            float currentAngle = Mathf.Lerp(0f, spinAngle * spinDirection, t);
            magazine.transform.rotation = Quaternion.Euler(0, 0, currentAngle);

            yield return null;
        }

        // Ensure the final scale and rotation are applied
        magazine.transform.localScale = targetScale;
        //magazine.transform.rotation = Quaternion.identity; // Reset to no rotation
    }


    //getters setters
    public float GetReloadTime()
    {
        return weaponData != null ? weaponData.reloadSpeed : 1f; // Default to 1s if no weapon data
    }


    private void ResetWeaponStats(WeaponData weaponData)
    {
        weaponData.reloadSpeed = weaponData.baseReloadSpeed;
        weaponData.fireRate = weaponData.baseFireRate;
        weaponData.spread = weaponData.baseSpread;
        weaponData.damage = weaponData.baseDamage;
        weaponData.apValue = weaponData.baseAPValue;
        weaponData.stoppingPower = weaponData.baseStoppingPower;
        weaponData.criticalChance = weaponData.baseCriticalChance;
    }

    private void ApplyMomentoMultipliers(WeaponData weaponData)
    {
        weaponData.apValue *= MomentoSystem.Instance.GetAPMultiplier();
        weaponData.damage += MomentoSystem.Instance.GetDamageMultiplier();
        weaponData.fireRate *= MomentoSystem.Instance.GetFireRateMultiplier();
        weaponData.reloadSpeed *= MomentoSystem.Instance.GetReloadSpeedMultiplier();
        weaponData.spread *= MomentoSystem.Instance.GetSpreadMultiplier();
        weaponData.criticalChance *= MomentoSystem.Instance.GetCriticalChanceMultiplier();
        weaponData.stoppingPower *= MomentoSystem.Instance.GetStoppingPowerMultiplier();
    }

    public void UpdateWeaponStats()
    {
        if (weaponData == null) return;

        // Reset weapon stats before applying new multipliers
        ResetWeaponStats(weaponData);

        // Apply the new momento multipliers
        ApplyMomentoMultipliers(weaponData);

        // Update UI if needed
        Debug.Log("Weapon stats updated due to momento pickup.");
        EventBus.Instance?.UpdateGunAnimationUI();
        EventBus.Instance?.UpdateAmmoUI(currentAmmo, weaponData.maxAmmo);
    }

    public void MeleeAttack()
    {
        GameObject meleeAttack = GameController.Instance.GetPooledObject("meleeArea", bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        MeleeAttack ma = meleeAttack.GetComponent<MeleeAttack>();
        ma.SetWeaponData(weaponData);
        //       MeleeAttackArea meleeAttack = Instantiate(meleeAttackPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

    }

}
