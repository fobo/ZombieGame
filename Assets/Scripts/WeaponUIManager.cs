using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // For TextMeshPro support

public class WeaponUIManager : MonoBehaviour
{
    public GameObject weaponSlotPrefab; // Assign the prefab in Unity Inspector
    public Transform weaponsPanel; // Assign the WeaponsPanel in Unity Inspector

    private void OnEnable() // When inventory UI opens, update it
    {
        UpdateWeaponUI();
    }

    public void UpdateWeaponUI()
    {
        // Clear previous weapon slots
        foreach (Transform child in weaponsPanel)
        {
            Destroy(child.gameObject);
        }

        // Get all weapons from InventorySystem
        Dictionary<string, WeaponData> weapons = InventorySystem.Instance.GetAllWeapons();

        foreach (var weaponEntry in weapons)
        {
            WeaponData weapon = weaponEntry.Value;

            // Instantiate a new weapon slot
            GameObject newWeaponSlot = Instantiate(weaponSlotPrefab, weaponsPanel);

            // Update UI components
            newWeaponSlot.transform.Find("WeaponIcon").GetComponent<Image>().sprite = weapon.weaponIdleFrame;
            newWeaponSlot.transform.Find("WeaponNameText").GetComponent<TMP_Text>().text = weapon.weaponName;
        }
    }
}
