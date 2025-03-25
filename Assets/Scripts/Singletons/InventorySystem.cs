using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; private set; } // Singleton instance

    private Dictionary<string, int> inventory = new Dictionary<string, int>();
    private Dictionary<string, WeaponData> weapons = new Dictionary<string, WeaponData>(); // keeps track of collected weapons
    private Dictionary<string, int> weaponMagazineAmmo = new Dictionary<string, int>();
    public Dictionary<AmmoType, int> ammoInventory = new Dictionary<AmmoType, int>();
    public Dictionary<CraftingType, int> craftingInventory = new Dictionary<CraftingType, int>();
    private List<Momento> momentoInventory = new List<Momento>();

    //icon dictionaries
    public Dictionary<CraftingType, Sprite> craftingIcons = new Dictionary<CraftingType, Sprite>();
    public Dictionary<AmmoType, Sprite> ammoIcons = new Dictionary<AmmoType, Sprite>();


    //icons
    [SerializeField] private Sprite woodIcon;
    [SerializeField] private Sprite metalIcon;
    [SerializeField] private Sprite electronicIcon;

    [SerializeField] private Sprite nineMMIcon;
    [SerializeField] private Sprite sevenSixTwoIcon;
    [SerializeField] private Sprite shotgunShellIcon;
    [SerializeField] private Sprite fiveFiveSixIcon;
    [SerializeField] private Sprite threeFiftySevenIcon;
    [SerializeField] private Sprite rocketIcon;
    [SerializeField] private Sprite defaultTurretIcon;
    private void Awake()
    {


        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps inventory across scenes
        }
        else
        {
            Debug.LogWarning("Duplicate InventorySystem detected. Destroying this instance.");
            Destroy(gameObject); // Destroys extra instances
            return;
        }

        foreach (AmmoType ammo in System.Enum.GetValues(typeof(AmmoType)))
        {
            ammoInventory[ammo] = 0; // set the default values to 0
        }
        foreach (CraftingType craftingType in System.Enum.GetValues(typeof(CraftingType)))
        {
            craftingInventory[craftingType] = 0; // set the default values to 0
        }


        craftingIcons[CraftingType.Wood] = woodIcon;
        craftingIcons[CraftingType.Metal] = metalIcon;
        craftingIcons[CraftingType.Electronic] = electronicIcon;

        ammoIcons[AmmoType.NineMM] = nineMMIcon;
        ammoIcons[AmmoType.SevenSixTwo] = sevenSixTwoIcon;
        ammoIcons[AmmoType.ShotgunShell] = shotgunShellIcon;
        ammoIcons[AmmoType.FiveFiveSix] = fiveFiveSixIcon;
        ammoIcons[AmmoType.ThreeFiftySeven] = threeFiftySevenIcon;
        ammoIcons[AmmoType.Rocket] = rocketIcon;
        ammoIcons[AmmoType.defaultTurret] = defaultTurretIcon;

    }

    private void Start()
    {
        // Subscribe to the event
        EventBus.Instance.OnAmmoUsed += UseAmmo;
        EventBus.Instance.OnAmmoAdded += AddAmmo;
        EventBus.Instance.OnMaterialAdded += AddCraftingMaterial;
        EventBus.Instance.OnWeaponAdded += AddWeapon;
        EventBus.Instance.OnMagazineSaved += SaveWeaponMagazineAmmo;
        EventBus.Instance.OnPrintInventory += PrintInventory;
    }

    //////////////////////ADD STUFF SECTION////////////////////////////////////
    /// <summary>
    /// Adds an item to the inventory. If it already exists, increase the count.
    /// </summary>
    public void AddItem(string itemName, int amount = 1)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName] += amount;
        }
        else
        {
            inventory[itemName] = amount;
        }

        Debug.Log($"{amount} {itemName}(s) added. New count: {inventory[itemName]}");
    }
    /// <summary>
    /// Adds a weapon to the player's inventory. If already owned, gives ammo instead.
    /// </summary>
    public void AddWeapon(WeaponData weapon)
    {
        if (!weapons.ContainsKey(weapon.weaponName))
        {
            weapons.Add(weapon.weaponName, weapon);
//            Debug.Log($"Picked up {weapon.weaponName}!");
            FindObjectOfType<WeaponUIManager>()?.UpdateWeaponUI();
        }
        else
        {
            Debug.Log($"{weapon.weaponName} already owned!");
        }
    }
    public void AddAmmo(AmmoType ammoType, int amount)
    {
        if (ammoInventory.ContainsKey(ammoType))
        {
            ammoInventory[ammoType] += amount;
        }
        else
        {
            ammoInventory[ammoType] = amount;
        }

        Debug.Log($"Picked up {amount} {ammoType} ammo. Total: {ammoInventory[ammoType]}");
    }

    public void AddCraftingMaterial(CraftingType craftingType, int amount)
    {
        //add the amount to the inventory, otherwise set it to the pickup amount (usually 1)
        if (craftingInventory.ContainsKey(craftingType))
        {
            craftingInventory[craftingType] += amount;
        }
        else
        {
            craftingInventory[craftingType] = amount;
        }
    }


    public void AddMomento(Momento momento)
    {
        momentoInventory.Add(momento);
        Debug.Log($"Added Momento to Inventory: {momento.momentoName} (Total Momentos: {momentoInventory.Count})");
    }
    /////////////////////////////////////////////////////////////
    ////////////////////REMOVE STUFF SECTION/////////////////////
    /// <summary>
    /// Removes an item from the inventory. Ensures the count never goes below zero.
    /// </summary>
    public bool RemoveItem(string itemName, int amount = 1)
    {
        if (inventory.ContainsKey(itemName) && inventory[itemName] >= amount)
        {
            inventory[itemName] -= amount;
            Debug.Log($"{amount} {itemName}(s) removed. Remaining: {inventory[itemName]}");

            if (inventory[itemName] <= 0)
            {
                inventory.Remove(itemName);
            }
            return true;
        }
        Debug.LogWarning($"Not enough {itemName} to remove!");
        return false;
    }
    /// <summary>
    /// Consumes ammo when a gun fires.
    /// </summary>
    public void UseAmmo(AmmoType ammoType, int amount)
    {
        if (HasAmmo(ammoType, amount))
        {
            ammoInventory[ammoType] -= amount;
        }
        else
        {
            Debug.LogWarning($"Not enough {ammoType} ammo!");
        }
    }

    public void UseCraftingMaterial(CraftingType craftingType, int amount)
    {
        if (HasCraftingMaterial(craftingType, amount))
        {
            craftingInventory[craftingType] -= amount;
        }
        else
        {
            Debug.LogWarning($"Not enough {craftingType}!");
        }
    }

    ///Crafting button

    //////////////////////////////////////////////////////////////////
    /// <summary>
    /// Checks if the inventory contains a certain item.
    /// </summary>
    public bool HasItem(string itemName, int requiredAmount = 1)
    {
        return inventory.ContainsKey(itemName) && inventory[itemName] >= requiredAmount;
    }
    /// <summary>
    /// Checks if there's enough ammo of the specified type.
    /// </summary>
    public bool HasAmmo(AmmoType ammoType, int amount)
    {
        return ammoInventory.ContainsKey(ammoType) && ammoInventory[ammoType] >= amount;
    }

    private bool HasCraftingMaterial(CraftingType craftingType, int amount)
    {//check if the crafting inventory has the correct type of material, and how much there is
        return craftingInventory.ContainsKey(craftingType) && craftingInventory[craftingType] >= amount;
    }
    /// <summary>
    /// Gets the amount of a specific item in the inventory.
    /// </summary>
    public int GetItemCount(string itemName)
    {
        return inventory.ContainsKey(itemName) ? inventory[itemName] : 0;
    }
    /// <summary>
    /// Get the current amount of ammo for a type.
    /// </summary>
    public int GetAmmoCount(AmmoType ammoType)
    {
        return ammoInventory.ContainsKey(ammoType) ? ammoInventory[ammoType] : 0;
    }
    /// <summary>
    /// Returns the entire inventory as a dictionary.
    /// </summary>
    public Dictionary<string, int> GetInventory()
    {
        return inventory;
    }

    /// <summary>
    /// Checks if a player owns a specific weapon.
    /// </summary>
    public bool HasWeapon(string weaponName)
    {
        return weapons.ContainsKey(weaponName);
    }

    /// <summary>
    /// Gets the total items in each inventory, usually to be displayed in the inventory UI.
    /// </summary>
    public Dictionary<AmmoType, int> GetAmmoInventory()
    {
        return ammoInventory;
    }

    public Dictionary<CraftingType, int> GetCraftingInventory()
    {
        return craftingInventory;
    }


    public WeaponData GetWeaponData(string weaponName)
    {
        if (weapons.ContainsKey(weaponName))
        {
            return weapons[weaponName];
        }
        return null;
    }
    /// <summary>
    /// Gets the entire list of owned weapons.
    /// </summary>
    public Dictionary<string, WeaponData> GetAllWeapons()
    {
        return weapons;
    }

    public List<Momento> GetMomentos()
    {
        return momentoInventory;
    }


    /////////////////////MAGAZINE STUFF////////////////////////
    /// <summary>
    /// Stores the remaining ammo for a weapon when switching.
    /// </summary>
    public void SaveWeaponMagazineAmmo(string weaponName, int currentAmmo)
    {
        weaponMagazineAmmo[weaponName] = currentAmmo;
    }

    /// <summary>
    /// Retrieves the saved ammo count for a weapon.
    /// </summary>
    public int GetSavedWeaponAmmo(string weaponName, int maxAmmo)
    {
        return weaponMagazineAmmo.ContainsKey(weaponName) ? weaponMagazineAmmo[weaponName] : maxAmmo;
    }








    /// <summary>
    /// Logs all items and weapons in the inventory. Used for legacy and bug testing.
    /// </summary>
    public void PrintInventory()
    {
        Debug.Log("===== INVENTORY CONTENTS =====");

        //  Print all consumable items (ammo, health, etc.)
        if (inventory.Count > 0)
        {
            Debug.Log("Items:");
            foreach (var item in inventory)
            {
                Debug.Log($"- {item.Key}: {item.Value}");
            }
        }
        else
        {
            Debug.Log("No consumable items.");
        }

        //  Print all weapons
        if (weapons.Count > 0)
        {
            Debug.Log("Weapons:");
            foreach (var weapon in weapons)
            {
                Debug.Log($"- {weapon}");
            }
        }
        else
        {
            Debug.Log("No weapons.");
        }

        //  Print all ammo counts
        if (ammoInventory.Count > 0)
        {
            Debug.Log("Ammo:");
            foreach (var ammo in ammoInventory)
            {
                Debug.Log($"- {ammo.Key}: {ammo.Value} rounds");
            }
        }
        else
        {
            Debug.Log("No ammo.");
        }

        Debug.Log("==============================");
    }


    public Sprite GetCraftingIcon(CraftingType type)
    {
        return craftingIcons.TryGetValue(type, out var icon) ? icon : null;
    }

    public Sprite GetAmmoIcon(AmmoType type)
    {
        return ammoIcons.TryGetValue(type, out var icon) ? icon : null;
    }

}
