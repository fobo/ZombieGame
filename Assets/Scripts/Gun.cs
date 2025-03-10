using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Weapon Data")]
    public WeaponData weaponData; // Reference to the weapon's data
    [Header("Bullet Settings")]
    public Transform bulletSpawnPoint; // The point where bullets spawn
    public GameObject bulletPrefab;    // The bullet prefab (if using physical bullets)


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
        EventBus.Instance.OnMomentoPickedUp += UpdateWeaponStats;
    }

    private void OnDisable()
    {
        EventBus.Instance.OnMomentoPickedUp -= UpdateWeaponStats;
    }


    /////////////// events
    public delegate void ReloadAction(float reloadTime);
    public event ReloadAction OnReloadStart;
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
            Text text = new Text("Reload!", Color.red);
            player.SpawnTextPopup(text);
        }
        if (isReloading || !canShoot || currentAmmo <= 0) return;


        currentAmmo--;

        int bulletsPerShot = (weaponData.fireType == FireType.Shotgun) ? weaponData.bulletsPerShot : 1;

        FireWeapon(bulletsPerShot);


        StartCoroutine(FireRateCooldown());

        // Update HUD
        EventBus.Instance?.UpdateAmmoUI(currentAmmo, weaponData.maxAmmo);
    }



    private void PlayShootAnimation()
    {
        if (gunAnimator != null)
        {
            gunAnimator.SetTrigger("Shoot");
        }
        else
        {
            Debug.LogWarning("Gun Animator not assigned!");
        }
    }

    private void PlayReloadAnimation()
    {
        gunAnimator.Play("Idle");
        if (gunAnimator != null)
        {
            gunAnimator.SetTrigger("Reload");
        }
        else
        {
            Debug.LogWarning("Gun Animator not assigned!");
        }
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

        PlayShootAnimation(); // Play shooting animation in the HUD
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
        PlayReloadAnimation();
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
            // Instantiate the magazine at the spawn point
            GameObject casing = Instantiate(weaponData.casingType, casingSpawnPoint.position, Quaternion.identity);

            // Start the scaling and spinning effect
            StartCoroutine(AnimateCasingEjection(casing));
        }
    }

    private IEnumerator AnimateCasingEjection(GameObject casing)
    {
        float ejectDuration = 0.5f; // Time the casing moves before physics takes over
        float elapsedTime = 0f;

        Rigidbody2D rb = casing.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = true; // Temporarily disable physics

            // Get the gun's rotation
            float gunAngle = transform.eulerAngles.z; // Get gun rotation in world space

            // Define the **base ejection direction** (rightward, assuming casings eject to the right)
            Vector2 baseEjectDirection = Vector2.right;

            // Rotate the ejection direction based on the gun's current angle
            Vector2 ejectDirection = Quaternion.Euler(0, 0, gunAngle - 90f + Random.Range(-15f, 15f)) * baseEjectDirection;

            // Set ejection speed
            float ejectSpeed = Random.Range(2f, 4f);

            // Apply an initial spin
            float spinSpeed = Random.Range(-300f, 300f);

            while (elapsedTime < ejectDuration)
            {
                elapsedTime += Time.deltaTime;

                // Move casing in rotated ejection direction
                casing.transform.position += (Vector3)(ejectDirection * ejectSpeed * Time.deltaTime);

                // Rotate casing for spin effect
                casing.transform.Rotate(0, 0, spinSpeed * Time.deltaTime);

                yield return null;
            }

            // Enable Rigidbody2D physics for natural falling
            rb.isKinematic = false;
            rb.velocity = ejectDirection * 1.5f; // Reduced velocity to avoid sliding too far
            rb.angularVelocity = spinSpeed / 2f; // Reduce spin for realism

            // Gradually slow down the casing's movement
            StartCoroutine(SlowDownCasing(rb));
        }
    }

    private IEnumerator SlowDownCasing(Rigidbody2D rb)
    {
        float stopDuration = 1.5f; // Time it takes for the casing to come to a stop
        float elapsedTime = 0f;

        Vector2 initialVelocity = rb.velocity;

        while (elapsedTime < stopDuration)
        {
            elapsedTime += Time.deltaTime;

            // Gradually reduce velocity
            rb.velocity = Vector2.Lerp(initialVelocity, Vector2.zero, elapsedTime / stopDuration);
            rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, 0f, elapsedTime / stopDuration);

            yield return null;
        }

        // Ensure it fully stops
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
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

            // Interpolate rotation (local Z-axis for 2D games)
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
        weaponData.damage *= MomentoSystem.Instance.GetDamageMultiplier();
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


}
