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

    [Header("Ammo Pickup Settings")]
    public AmmoType ammoType;

    [Header("Material Type Settings")]
    public CraftingType craftingType;

    [Header("Weapon Pickup Settings")]
    public WeaponData weaponData; // Assign if it's a weapon pickup
}
