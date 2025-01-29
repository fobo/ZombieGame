using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Weapon Data")]
    public WeaponData weaponData; // Reference to the weapon's data

    [Header("Bullet Settings")]
    public Transform bulletSpawnPoint; // The point where bullets spawn
    public GameObject bulletPrefab;    // The bullet prefab (if using physical bullets)

    public GameObject magazinePrefab; // Magazine prefab to drop
    public Transform magazineSpawnPoint; // Where the magazine spawns (e.g., under the gun)

    public GameObject casingPrefab; // shell casing
    public Transform casingSpawnPoint; // where the casing will spawn (like a minigun will be more to the left, etc)

    [Header("Runtime Variables")]
    private int currentAmmo;
    private bool isReloading;
    private bool canShoot = true;

    private void Start()
    {
        currentAmmo = weaponData.maxAmmo;
    }

    public void Shoot()
    {
        if (isReloading || !canShoot || currentAmmo <= 0) return;

        currentAmmo--;

        if (weaponData.fireType == FireType.Shotgun)
        {
            for (int i = 0; i < weaponData.bulletsPerShot; i++)
            {
                FireBulletWithSpread();
            }
        }
        else
        {
            FireBulletWithSpread();
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }
        else if (weaponData.fireType == FireType.Automatic)
        {
            StartCoroutine(FireRateCooldown());
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
            Debug.Log("Bullet spawned at " + bulletSpawnPoint.position + " And rotation " + bulletSpawnPoint.rotation);
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction.normalized * 20f; // Adjust speed as needed
            }
        }
        EjectShellCasing();
        Debug.Log($"Bullet fired with spread in direction: {direction}");
    }

    public IEnumerator Reload()
    {
        if (isReloading) yield break;
        isReloading = true;

        Debug.Log($"Reloading {weaponData.reloadType}...");

        switch (weaponData.reloadType)
        {
            case ReloadType.Magazine:
                yield return new WaitForSeconds(weaponData.reloadSpeed);
                currentAmmo = weaponData.maxAmmo;
                DropMagazine();
                break;

            case ReloadType.SingleShot:
            case ReloadType.Tube:
                while (currentAmmo < weaponData.maxAmmo)
                {
                    yield return new WaitForSeconds(weaponData.reloadSpeed / weaponData.maxAmmo);
                    currentAmmo++;
                    Debug.Log($"Reloaded 1 bullet. Ammo: {currentAmmo}/{weaponData.maxAmmo}");
                }
                break;
        }

        isReloading = false;
        Debug.Log("Reload complete!");
    }

    private IEnumerator FireRateCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(weaponData.fireRate);
        canShoot = true;
    }

    private void DropMagazine()
    {
        if (magazinePrefab != null && magazineSpawnPoint != null)
        {
            // Instantiate the magazine at the spawn point
            GameObject magazine = Instantiate(magazinePrefab, magazineSpawnPoint.position, Quaternion.identity);

            // Start the scaling and spinning effect
            StartCoroutine(AnimateMagazineDrop(magazine));
        }
    }

    private void EjectShellCasing()
    {
        if (casingPrefab != null && casingSpawnPoint != null)
        {
            // Instantiate the magazine at the spawn point
            GameObject casing = Instantiate(casingPrefab, casingSpawnPoint.position, Quaternion.identity);

            // Start the scaling and spinning effect
            StartCoroutine(AnimateCasingDrop(casing));
        }
    }

    private IEnumerator AnimateCasingDrop(GameObject casing)
    {
        float duration = 0.2f; // Total time for the animation
        float elapsedTime = 0f;

        // Initial and target scales
        Vector3 initialScale = casing.transform.localScale * 1.5f; // 150% size
        Vector3 targetScale = casing.transform.localScale;         // 100% size

        // Random spin angle (30 to 60 degrees)
        float spinAngle = Random.Range(5f, 355f);
        float spinDirection = Random.value > 0.5f ? 1f : -1f; // Randomize clockwise/counterclockwise spin

        // Animate scaling and spinning over time
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Interpolate scale
            float t = elapsedTime / duration;
            casing.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            // Interpolate rotation (local Z-axis for 2D games)
            float currentAngle = Mathf.Lerp(0f, spinAngle * spinDirection, t);
            casing.transform.rotation = Quaternion.Euler(0, 0, currentAngle);

            yield return null;
        }

        // Ensure the final scale and rotation are applied
        casing.transform.localScale = targetScale;
        //magazine.transform.rotation = Quaternion.identity; // Reset to no rotation
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


    public int GetCurrentAmmo() => currentAmmo;
}
