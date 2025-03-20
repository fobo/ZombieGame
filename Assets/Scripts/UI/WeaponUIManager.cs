using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; // For TextMeshPro support

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

            // Add EventTriggers for hover detection
            AddTooltipTriggers(newWeaponSlot, weapon);
        }
    }

    private void AddTooltipTriggers(GameObject weaponSlot, WeaponData weapon)
    {
        EventTrigger trigger = weaponSlot.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = weaponSlot.AddComponent<EventTrigger>();
        }
        trigger.triggers.Clear();

        string tooltipText = $"{weapon.weaponName}\n{(string.IsNullOrEmpty(weapon.description) ? "No description available" : weapon.description)}";

        // Pointer Enter (Show Tooltip)
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((eventData) =>
        {
            if (TooltipManager.Instance != null)
            {
                TooltipManager.Instance.ShowTooltip(tooltipText, weaponSlot.transform);
            }
        });
        trigger.triggers.Add(entryEnter);

        // Pointer Exit (Hide Tooltip)
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((eventData) =>
        {
            if (TooltipManager.Instance != null)
            {
                TooltipManager.Instance.HideTooltip();
            }
        });
        trigger.triggers.Add(entryExit);
    }


}
