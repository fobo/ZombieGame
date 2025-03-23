using UnityEngine;

[CreateAssetMenu(fileName = "NewPickup", menuName = "Pickups/Pickup Data")]
public class PickupData : ScriptableObject
{
    [Header("General Info")]
    public string pickupName;
    public Sprite pickupIcon;
    public PickupType pickupType;

    [Header("Amount of the item")]
    public int amount; // Health or ammo amount, or whatever.

    [Header("Consumable Pickup Settings")]
    public AmmoType ammoType; // "Ammo" is a generic term here, since you will "fire" turrets onto the ground, or "fire" medkits into your bloodstream.

    [Header("Material Type Settings")]
    public CraftingType craftingType;

    [Header("Weapon Pickup Settings")]
    public WeaponData weaponData; // Assign if it's a weapon pickup
}
