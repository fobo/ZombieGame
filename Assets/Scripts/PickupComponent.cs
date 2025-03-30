using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PickupData pickupData; // scriptable object
    public SpriteRenderer sr; // reference to the sprite renderer component
    public AudioClip weaponPickupSFX; //sound clip of the weapon pickup effect

    void Awake()
    {
        //apply shaders to the pickups
        if (GetComponent<PickupShaderApplier>() == null)
        {
            //gameObject.AddComponent<PickupShaderApplier>();
        }
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = pickupData.pickupIcon;
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
        PlayerController pc = player.GetComponent<PlayerController>();
        Text text = new Text(pickupData.pickupName, Color.green);
        pc.SpawnTextPopup(text);
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


                }
                break;

            case PickupType.Weapon:
                if (InventorySystem.Instance != null && pickupData.weaponData != null)
                {
                    SFXManager.Instance.PlaySFXClip(weaponPickupSFX, gameObject.transform, 0.5f); // play weapon pickup SFX
                    InventorySystem.Instance.AddWeapon(pickupData.weaponData);

                    PlayerController pcWeapon = player.GetComponent<PlayerController>();
                    if (pcWeapon != null && pcWeapon.equippedGun != null)
                    {
                        pcWeapon.SwitchWeapon(pickupData.weaponData);
                    }
                }
                break;
            case PickupType.Momento:
                Momento momento = GetComponent<Momento>();
                if (momento != null)
                {
                    //InventorySystem.Instance.AddMomento(momento);
                    MomentoSystem.Instance.AddMomento(momento);
                    EventBus.Instance?.MomentoPickedUp();
                    //                   Debug.Log($"Picked up Momento: {momento.momentoName}");
                }
                else
                {
                    Debug.LogError($"Momento script is missing from {gameObject.name}!");
                }
                break;
            case PickupType.Crafting:
                InventorySystem.Instance.AddCraftingMaterial(pickupData.craftingType, pickupData.amount);
                //               Debug.Log($"Picked up {pickupData.amount} {pickupData.craftingType}.");
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
    Crafting,
    Custom
}
