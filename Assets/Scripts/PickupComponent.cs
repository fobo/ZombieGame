using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PickupData pickupData; // Now uses a ScriptableObject


    void Awake()
    {
        //apply shaders to the pickups
        if (GetComponent<PickupShaderApplier>() == null)
        {
            gameObject.AddComponent<PickupShaderApplier>();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyPickup(other.gameObject);
            Destroy(gameObject); // Destroy the pickup after collection

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

            //add a method we can call to display a pickup text on the scene for the player
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
            case PickupType.Momento:
                Momento momento = GetComponent<Momento>();
                if (momento != null)
                {
                    //InventorySystem.Instance.AddMomento(momento);
                    MomentoSystem.Instance.AddMomento(momento);
                    EventBus.Instance?.MomentoPickedUp();
                    Debug.Log($"Picked up Momento: {momento.momentoName}");
                }
                else
                {
                    Debug.LogError($"Momento script is missing from {gameObject.name}!");
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
    Momento,
    Custom
}
