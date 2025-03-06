using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventBus : MonoBehaviour
{
    public static EventBus Instance { get; private set; }

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    //  Events for inventory changes
    public event Action<AmmoType, int> OnAmmoUsed;
    public event Action<AmmoType, int> OnAmmoAdded;
    public event Action<WeaponData> OnWeaponAdded;
    public event Action<string, int> OnMagazineSaved;
    public event Action OnPrintInventory;


    //Events for HUD updates
    public event Action<int, int> OnAmmoUpdated;  // (currentAmmo, maxAmmo)
    public event Action OnEquipWeapon; 


    public Delegate GetOnEquipWeaponDelegate()
    {
        return OnEquipWeapon;
    }
    /// <summary>
    ///  Fire event when ammo is used
    /// </summary>
    public void UseAmmo(AmmoType ammoType, int amount)
    {
        OnAmmoUsed?.Invoke(ammoType, amount);
    }

    /// <summary>
    ///  Fire event when ammo is added
    /// </summary>
    public void AddAmmo(AmmoType ammoType, int amount)
    {
        OnAmmoAdded?.Invoke(ammoType, amount);
    }

    /// <summary>
    ///  Fire event when a weapon is picked up
    /// </summary>
    public void AddWeapon(WeaponData weapon)
    {
        OnWeaponAdded?.Invoke(weapon);
    }

    /// <summary>
    ///  Fire event when weapon magazine ammo is saved
    /// </summary>
    public void SaveWeaponMagazineAmmo(string weaponName, int currentAmmo)
    {
        OnMagazineSaved?.Invoke(weaponName, currentAmmo);
    }

    /// <summary>
    ///  Fire event when printing inventory
    /// </summary>
    public void PrintInventory()
    {
        OnPrintInventory?.Invoke();
    }

    /// <summary>
    ///  Fire event when ammo count changes
    /// </summary>
    public void UpdateAmmoUI(int currentAmmo, int maxAmmo)
    {
        OnAmmoUpdated?.Invoke(currentAmmo, maxAmmo);
    }


    /// <summary>
    ///  Fire event when a new weapon is equipped
    /// </summary>
    public void UpdateGunAnimationUI()
    {
        Debug.Log("Updating ui");
        OnEquipWeapon?.Invoke();
    }

}
