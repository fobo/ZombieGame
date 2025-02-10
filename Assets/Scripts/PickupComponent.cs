// using UnityEngine;

// public class Pickup : MonoBehaviour
// {
//     [Header("Pickup Settings")]
//     public PickupType pickupType; // Determines what kind of item this is
//     public int amount = 10; // Amount to add (ammo, health, etc.)
//     public WeaponData weaponData;

//     [Header("Ammo Settings")]
//     public AmmoType ammoType;



//     private void OnTriggerEnter2D(Collider2D other)



//     {
//         if (other.CompareTag("Player"))
//         {
//             ApplyPickup(other.gameObject);
//             Destroy(gameObject); // Destroy the pickup after collection
//             InventorySystem.Instance.PrintInventory();
//         }
//     }

//     private void ApplyPickup(GameObject player)
//     {
//         switch (pickupType)
//         {
//             case PickupType.Health:
//                 HealthComponent playerHealth = player.GetComponent<HealthComponent>();
//                 if (playerHealth != null)
//                 {
//                     playerHealth.Heal(amount);
//                 }
//                 break;

//             case PickupType.Ammo:
//                 if (InventorySystem.Instance != null)
//                 {
//                     InventorySystem.Instance.AddAmmo(ammoType, amount);
//                     Debug.Log($"Picked up {amount} {ammoType} ammo.");
//                 }
//                 else
//                 {
//                     Debug.LogError("InventorySystem Instance is NULL! Ensure it exists in the scene.");
//                 }
//                 break;


//             case PickupType.Weapon:
//                 if (InventorySystem.Instance != null && weaponData != null)
//                 {
//                     InventorySystem.Instance.AddWeapon(weaponData);
//                 }
//                 break;

//             case PickupType.Custom:

//                 break;
//         }
//     }
// }



using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PickupData pickupData; // Now uses a ScriptableObject

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
        if (pickupData == null)
        {
            Debug.LogError("PickupData is NULL! Assign a PickupData asset in the Inspector.");
            return;
        }

        switch (pickupData.pickupType)
        {
            case PickupType.Health:
                HealthComponent playerHealth = player.GetComponent<HealthComponent>();
                if (playerHealth != null)
                {
                    playerHealth.Heal(pickupData.amount);
                }
                break;

            case PickupType.Ammo:
                if (InventorySystem.Instance != null)
                {
                    InventorySystem.Instance.AddAmmo(pickupData.ammoType, pickupData.amount);
                    Debug.Log($"Picked up {pickupData.amount} {pickupData.ammoType} ammo.");
                }
                break;

            case PickupType.Weapon:
                if (InventorySystem.Instance != null && pickupData.weaponData != null)
                {
                    InventorySystem.Instance.AddWeapon(pickupData.weaponData);
                }
                break;

            case PickupType.Custom:
                // Add custom pickup logic here
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
