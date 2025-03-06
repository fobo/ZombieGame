using UnityEngine;

// Holds data for each weapon
[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Animation Data")]
    public RuntimeAnimatorController weaponAnimation; // The weapons animation controller goes here.
    public Sprite weaponIdleFrame; // This is the weapons default idle frame used for animations.
    [Header("General Settings")]
    public string weaponName; // Weapon name (e.g., AK-47)
    public Sprite weaponSprite; // Visual representation of the weapon
    public FireType fireType; // Firing type (e.g., Automatic, SingleFire, Shotgun)
    public ReloadType reloadType; // Reload type (e.g., Magazine, Tube)
    public GameObject casingType; // the sprite that represents the casing the gun ejects (shell, rifle rocket tube, etc)
    public GameObject magazineType; // the sprite that represents the MAGAZINE the gun drops when reloading (if applicable, tube shotguns do not have these.)

    [Header("Weapon Stats")]
    public int maxAmmo = 30; // Maximum ammo capacity
    public float reloadSpeed = 2f; // Time it takes to reload
    public float fireRate = 0.1f; // Time between shots (Automatic fire only)
    public float spread = 0.1f; // Bullet inaccuracy (0 = no spread)
    public int bulletsPerShot = 1; // Number of bullets fired per shot (e.g., for shotguns)
    public float damage = 10; // Damage per bullet
    public AmmoType ammoType; // Choose the ammo type.... be careful, you can make the shotgun shoot rockets if you wanted it to.
    public float apValue; // armor piercing value, i.e how good is it at piercing armor

    [TextArea]
    public string description; // Optional description for the weapon
}
