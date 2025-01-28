using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Weapon Data")]
    public WeaponData weaponData; // Reference to the weapon's data

    [Header("Runtime Variables")]
    private int currentAmmo; // Tracks current ammo in the magazine
    private bool isReloading; // Prevents shooting while reloading
    private bool canShoot = true; // Prevents spamming fire beyond fire rate

    private void Start()
    {
        currentAmmo = weaponData.maxAmmo; // Initialize ammo
    }

    public void Shoot()
    {
        if (isReloading || !canShoot || currentAmmo <= 0) return;

        currentAmmo--;

        // Handle different firing types
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

        Debug.Log($"{weaponData.weaponName} fired! Ammo left: {currentAmmo}");

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
        Vector3 direction = transform.forward + new Vector3(randomSpreadX, randomSpreadY, 0);

        // Instantiate or simulate the bullet
        Debug.Log($"Bullet fired in direction: {direction}. Damage: {weaponData.damage}");
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

    public int GetCurrentAmmo() => currentAmmo;
}
