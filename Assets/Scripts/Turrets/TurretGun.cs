using System.Collections;
using UnityEngine;

public class TurretGun : MonoBehaviour
{
    [Header("Weapon Data")]
    public WeaponData weaponData; // Reference to the weapon's data
    [Header("Bullet Settings")]
    public Transform bulletSpawnPoint; // The point where bullets spawn
    public GameObject bulletPrefab;    // The bullet prefab (if using physical bullets)




    public Transform casingSpawnPoint; // where the casing will spawn (like a minigun will be more to the left, etc)


    private int currentAmmo;

    [Header("Runtime Variables")]
    private bool canShoot = true;


    void Start()
    {
        currentAmmo = 75;
    }

    public void Shoot()
    {
        if (!canShoot) return;


        currentAmmo--;

        if (currentAmmo <= 0)
        {
            Destroy(transform.root.gameObject);

        }
        int bulletsPerShot = (weaponData.fireType == FireType.Shotgun) ? weaponData.bulletsPerShot : 1;

        FireWeapon(bulletsPerShot);


        StartCoroutine(FireRateCooldown());
    }




    private void FireWeapon(int bulletsPerShot)
    {
        for (int i = 0; i < bulletsPerShot; i++)
        {
            // Random spread angle in degrees
            //float spreadAngle = Random.Range(-weaponData.spread, weaponData.spread);

            // Calculate the new bullet rotation based on the gun's current rotation
            Quaternion spreadRotation = Quaternion.Euler(0, 0, Random.Range(-weaponData.spread, weaponData.spread));
            Quaternion finalRotation = bulletSpawnPoint.rotation * spreadRotation;

            // Determine the final firing direction
            Vector2 finalDirection = spreadRotation * transform.forward;

            if (bulletSpawnPoint != null)
            {


                // Get a bullet from the pool instead of Instantiating
                GameObject bullet = GameController.Instance.GetPooledObject("Bullet", bulletSpawnPoint.position, finalRotation);


                if (bullet != null)
                {
                    Bullet bs = bullet.GetComponent<Bullet>();
                    bs.SetWeaponData(weaponData); // apply the current weapon data to the bullet

                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.velocity = transform.forward * 20f; // Set bullet speed
                    }
                }
            }
        }
        EjectShellCasing(); // Eject shell casing (only one per shot)
    }





    private IEnumerator FireRateCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(weaponData.fireRate * MomentoSystem.Instance.GetFireRateMultiplier()); // increases "fire rate"
        canShoot = true;
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
            Vector2 ejectDirection = Quaternion.Euler(0, 0, gunAngle + 90f + Random.Range(-15f, 15f)) * baseEjectDirection;

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

    }


}
