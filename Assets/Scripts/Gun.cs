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
    private bool isCritical = false;



    // BUILT IN METHODS
    private void Start()
    {

        if (gunAnimator == null)
        {
            gunAnimator = GetComponentInChildren<Animator>();
        }

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

    public void EquipWeapon(WeaponData newWeaponData)
    {
        //  Save current weapon's magazine ammo before switching
        if (weaponData != null)
        {
            InventorySystem.Instance.SaveWeaponMagazineAmmo(weaponData.weaponName, currentAmmo);
        }

        //  Assign the new weapon data
        weaponData = newWeaponData;

        //  Retrieve the saved ammo count (or use max ammo if new weapon)
        currentAmmo = InventorySystem.Instance.GetSavedWeaponAmmo(weaponData.weaponName, weaponData.maxAmmo);

        //  Update the HUD with the correct ammo
        Debug.Log("Updating from gun");
        EventBus.Instance?.UpdateGunAnimationUI();
        EventBus.Instance?.UpdateAmmoUI(currentAmmo, weaponData.maxAmmo);
    }




    public void Shoot()
    {
        if (isReloading || !canShoot || currentAmmo <= 0) return;


        currentAmmo--;


        if (weaponData.fireType == FireType.Shotgun)
        {
            FireShellWithSpread();
        }
        else
        {
            FireBulletWithSpread();
        }

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

    private void FireBulletWithSpread()
    {
        // Random spread angle in degrees
        float spreadAngle = Random.Range(-weaponData.spread, weaponData.spread);

        // Calculate the new bullet rotation based on the gun's current rotation
        Quaternion spreadRotation = Quaternion.Euler(0, 0, spreadAngle);

        // Determine the final firing direction
        Vector2 finalDirection = spreadRotation * transform.right;

        if (bulletSpawnPoint != null)
        {
            float randChance = Random.Range(0f, 1f);//return a number between 0 and 1

            if (GameDirector.Instance.GetCriticalChance() > randChance)
            {
                Debug.Log("Critical!!!!");
                isCritical = true; //next bullet generated is a critical hit!
            }
            //  Get a bullet from the pool instead of Instantiating
            GameObject bullet = GameController.Instance.GetPooledObject("Bullet", bulletSpawnPoint.position, bulletSpawnPoint.rotation * spreadRotation);

            if (bullet != null)
            {
                Bullet bs = bullet.GetComponent<Bullet>(); // gain access to the bullet script
                if(isCritical){bs.setCritical();}// sets the bullet to be critical
                bs.SetWeaponData(weaponData);

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = finalDirection * 20f; // Set bullet speed
                }
            }
            isCritical = false; // turn off crits after every shot
        }

        PlayShootAnimation(); // plays the shooting animation in the HUD
        EjectShellCasing();
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

            if (bulletSpawnPoint != null)
            {
                float randChance = Random.Range(0f, 1f);//return a number between 0 and 1
                
                if (GameDirector.Instance.GetCriticalChance() > randChance)
                {
                    isCritical = true; //next bullet generated is a critical hit!
                }
                //  Get a bullet from the pool instead of Instantiating
                GameObject bullet = GameController.Instance.GetPooledObject("Bullet", bulletSpawnPoint.position, bulletSpawnPoint.rotation * spreadRotation);

                if (bullet != null)
                {
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    Bullet bs = bullet.GetComponent<Bullet>();
                    if(isCritical){bs.setCritical();}
                    bs.SetWeaponData(weaponData);
                    if (rb != null)
                    {
                        rb.velocity = finalDirection * 20f; // Set bullet speed
                    }
                }
                isCritical = false;
            }
        }

        PlayShootAnimation(); // Play shooting animation
        EjectShellCasing(); // Shotgun should eject ONE shell per shot
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
