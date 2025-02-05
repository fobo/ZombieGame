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

        currentAmmo = weaponData.maxAmmo; // Start full mag
        HUDController.Instance?.UpdateAmmoUI(currentAmmo, weaponData.maxAmmo);
        //old ammo component code, not used for the player anymore.
        // if (ammoComponent == null)
        // {
        //     ammoComponent = GetComponentInParent<AmmoComponent>(); // Auto-assign if not set in the Inspector
        // }
    }

    void Update()
    {
        // Check if the player presses "R" and the gun is not already reloading
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    ///////////////


    public void Shoot()
    {
        // Ensure inventory system exists and we have ammo
        if (isReloading || !canShoot || currentAmmo <= 0) return;

        currentAmmo--;

        if (weaponData.fireType == FireType.Shotgun)
        {
            Debug.Log("Firing shotgun");
            FireShellWithSpread(); // fire shotgun
        }
        else
        {
            FireBulletWithSpread(); //fire literally everything else (for now)
        }


        StartCoroutine(FireRateCooldown()); // most guns need to re acquire target before shooting again.


        // Update the HUD with new ammo count
        HUDController.Instance?.UpdateAmmoUI(currentAmmo, weaponData.maxAmmo);
        //gunHUD.UpdateAmmoUI(InventorySystem.Instance.GetItemCount("Ammo"), weaponData.maxAmmo);
    }

    /// <summary>
    /// Switch to a new weapon.
    /// </summary>
    public void EquipWeapon(WeaponData newWeapon)
    {
        weaponData = newWeapon;
        currentAmmo = weaponData.maxAmmo; // Reload new weapon
        Debug.Log($"Equipped {weaponData.weaponName}");
        //Link the gun hud to the gun script so we can call UpdateGunAnimationUI()
        //UpdateHUD();
        HUDController.Instance?.UpdateGunAnimationUI();
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

    private void FireBulletWithSpread()
    {
        // Apply spread
        float randomSpreadX = Random.Range(-weaponData.spread, weaponData.spread);
        float randomSpreadY = Random.Range(-weaponData.spread, weaponData.spread);

        Vector3 direction = transform.right + new Vector3(randomSpreadX, randomSpreadY, 0);

        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            // Spawn the bullet at the bulletSpawnPoint
            //Debug.Log("Bullet spawned at " + bulletSpawnPoint.position + " And rotation " + bulletSpawnPoint.rotation);
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction.normalized * 20f; // Adjust speed as needed
            }
        }
        PlayShootAnimation(); // plays the shooting animation in the HUD
        //gunHUD.PlayShellEjectionAnimation(); // plays the shell ejection from the HUD
        EjectShellCasing();
        //Debug.Log($"Bullet fired with spread in direction: {direction}");
    }

    //call this fire with spread when using a shotgun. shots is the number of projectiles each shell contains.
    private void FireShellWithSpread()
    {
        for (int i = 0; i < weaponData.bulletsPerShot; i++)
        {
            // Random spread angle in degrees
            float spreadAngle = Random.Range(-weaponData.spread, weaponData.spread);

            // Calculate the new bullet rotation based on the gun's current rotation
            Quaternion spreadRotation = Quaternion.Euler(0, 0, spreadAngle);

            // Determine the final firing direction
            Vector2 finalDirection = spreadRotation * transform.right;

            if (bulletPrefab != null && bulletSpawnPoint != null)
            {
                // Instantiate the bullet and adjust its rotation
                GameObject bullet = Instantiate(
                    bulletPrefab,
                    bulletSpawnPoint.position,
                    bulletSpawnPoint.rotation * spreadRotation // Apply spread rotation to bullet
                );

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = finalDirection * 20f;
                }
            }
        }

        PlayShootAnimation(); // Play shooting animation
        EjectShellCasing(); // Eject one shell per shotgun shot
    }




    public IEnumerator Reload()
    {
        if (isReloading) yield break;
        isReloading = true;
        PlayReloadAnimation();
        Debug.Log($"Reloading {weaponData.reloadType}...");

        //  Get how much ammo is missing in the magazine
        int missingAmmo = weaponData.maxAmmo - currentAmmo;

        //  Check how much is available in reserve
        int ammoReserve = InventorySystem.Instance.GetItemCount("Ammo");

        //  Take only what's needed
        int ammoToReload = Mathf.Min(missingAmmo, ammoReserve);

        switch (weaponData.reloadType)
        {
            case ReloadType.Magazine:
                Invoke("DropMagazine", weaponData.reloadSpeed / 2);
                yield return new WaitForSeconds(weaponData.reloadSpeed);
                InventorySystem.Instance.RemoveItem("Ammo", ammoToReload);
                currentAmmo += ammoToReload;
                break;

            case ReloadType.SingleShot:
            case ReloadType.Tube:
                while (ammoToReload > 0)
                {
                    Debug.Log(ammoToReload);
                    //shotgun loads one shell at a time, so reload speed should be pretty short.
                    yield return new WaitForSeconds(weaponData.reloadSpeed);
                    // Implement this later
                    Debug.Log("Loading Shells " + currentAmmo + "/" + weaponData.maxAmmo);// "Loading shells 3/8" etc
                    InventorySystem.Instance.RemoveItem("Ammo", 1);
                    currentAmmo++; // add a shell to the tube
                    ammoToReload--; // remove a shell from the waiting reserve
                    HUDController.Instance?.UpdateAmmoUI(currentAmmo, weaponData.maxAmmo); // show the hud updating as we load in shells
                    //we can add a feature to cancel reloading early just in case we want to shoot before the tube is full.

                }
                break;
        }

        isReloading = false;
        Debug.Log("Reload complete!");
        HUDController.Instance?.UpdateAmmoUI(currentAmmo, weaponData.maxAmmo);
        //gunHUD.UpdateAmmoUI(InventorySystem.Instance.GetItemCount("Ammo"), weaponData.maxAmmo);
    }

    private IEnumerator FireRateCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(weaponData.fireRate);
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


}
