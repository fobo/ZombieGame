using UnityEngine;

public class AmmoComponent : MonoBehaviour
{
    [Header("Ammo Settings")]
    public int maxAmmo = 30; // Maximum ammo capacity
    public int currentAmmo;  // Current ammo count
    public bool infiniteAmmo = false; // Optional setting for turrets with unlimited ammo

    private void Start()
    {
        // Initialize ammo to full at the start
        currentAmmo = maxAmmo;
    }

    /// <summary>
    /// Checks if there is ammo available.
    /// </summary>
    public bool HasAmmo()
    {
        return infiniteAmmo || currentAmmo > 0;
    }

    /// <summary>
    /// Consumes ammo when shooting.
    /// </summary>
    public bool UseAmmo(int amount = 1)
    {
        
        if (!HasAmmo()) return false;

        if (!infiniteAmmo)
        {
            currentAmmo = Mathf.Max(currentAmmo - amount, 0);
        }

        if(gameObject.CompareTag("Player")){
            Debug.Log("Ammo: " + currentAmmo + "/" + maxAmmo);
        }
        return true;
    }

    /// <summary>
    /// Refills ammo up to max capacity.
    /// </summary>
    public void RefillAmmo(int amount)
    {
        if (!infiniteAmmo)
        {
            currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
        }
                if(gameObject.CompareTag("Player")){
            Debug.Log("Ammo: " + currentAmmo + "/" + maxAmmo);
        }
    }

    /// <summary>
    /// Fully reloads ammo to max.
    /// </summary>
    public void Reload()
    {
        if (!infiniteAmmo)
        {
            currentAmmo = maxAmmo;
        }
    }
}
