using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; private set; } // Singleton instance

    private Dictionary<string, int> inventory = new Dictionary<string, int>();

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
    }

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
    /// Checks if the inventory contains a certain item.
    /// </summary>
    public bool HasItem(string itemName, int requiredAmount = 1)
    {
        return inventory.ContainsKey(itemName) && inventory[itemName] >= requiredAmount;
    }

    /// <summary>
    /// Gets the amount of a specific item in the inventory.
    /// </summary>
    public int GetItemCount(string itemName)
    {
        return inventory.ContainsKey(itemName) ? inventory[itemName] : 0;
    }

    /// <summary>
    /// Returns the entire inventory as a dictionary.
    /// </summary>
    public Dictionary<string, int> GetInventory()
    {
        return inventory;
    }
}
