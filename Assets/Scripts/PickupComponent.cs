using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public PickupType pickupType; // Determines what kind of item this is
    public int amount = 10; // Amount to add (ammo, health, etc.)
    public WeaponData weaponData;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyPickup(other.gameObject);
            Destroy(gameObject); // Destroy the pickup after collection
            InventorySystem.Instance.PrintInventory();
        }
    }

    private void ApplyPickup(GameObject player)
    {
        switch (pickupType)
        {
            case PickupType.Health:
                HealthComponent playerHealth = player.GetComponent<HealthComponent>();
                if (playerHealth != null)
                {
                    playerHealth.Heal(amount);
                }
                break;

            case PickupType.Ammo:
                if (InventorySystem.Instance != null)
                {
                    InventorySystem.Instance.AddItem("Ammo", amount);
                    Debug.Log($"Picked up {amount} Ammo. Total: {InventorySystem.Instance.GetItemCount("Ammo")}");
                }
                else
                {
                    Debug.LogError("InventorySystem Instance is NULL! Ensure it exists in the scene.");
                }
                break;


            case PickupType.Weapon:
                if (InventorySystem.Instance != null && weaponData != null)
                {
                    InventorySystem.Instance.AddWeapon(weaponData);
                }
                break;

            case PickupType.Custom:

                break;
        }
    }
}

// Enum for different pickup types
public enum PickupType
{
    Ammo,
    Health,
    Weapon,
    Custom
}
