using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; // For tooltip triggers

public class CraftingUIManager : MonoBehaviour
{
    public GameObject craftingSlotPrefab; // Assign in Unity Inspector
    public Transform craftingMaterialsPanel; // Assign in Unity Inspector

    [Header("Ammo Icons (Assign in Inspector)")]
    public Sprite WoodScrapIcon;
    public Sprite MetalScrapIcon;
    public Sprite ElectronicScrapIcon;


    private Dictionary<CraftingType, Sprite> craftingIcons;

    private void Awake()
    {
        craftingIcons = new Dictionary<CraftingType, Sprite>
        {
            { CraftingType.Wood, WoodScrapIcon },
            { CraftingType.Metal, MetalScrapIcon },
            { CraftingType.Electronic, ElectronicScrapIcon }

        };
    }

    private void OnEnable()
    {
        UpdateCraftingUI();
    }

    public void UpdateCraftingUI()
    {

        if (craftingSlotPrefab == null || craftingMaterialsPanel == null)
        {
            Debug.LogError("CraftingUIManager: Missing prefab or panel reference!");
            return;
        }

        // Clear previous crafting slots
        foreach (Transform child in craftingMaterialsPanel)
        {
            Destroy(child.gameObject);
        }

        // Get all material counts from InventorySystem
        Dictionary<CraftingType, int> materialCounts = InventorySystem.Instance.GetCraftingInventory();

        foreach (var materialEntry in materialCounts)
        {
            CraftingType craftingType = materialEntry.Key;
            int craftCount = materialEntry.Value;

            // Instantiate a new material slot
            GameObject newMaterialSlot = Instantiate(craftingSlotPrefab, craftingMaterialsPanel);

            // Update UI components
            newMaterialSlot.transform.Find("MaterialIcon").GetComponent<Image>().sprite = GetMaterialIcon(craftingType);
            newMaterialSlot.transform.Find("MaterialCountText").GetComponent<TMP_Text>().text = $"x{craftCount}";

            // Add tooltip triggers
            AddTooltipTriggers(newMaterialSlot, craftingType, craftCount);
        }
    }

    private Sprite GetMaterialIcon(CraftingType craftingType)
    {
        if (craftingIcons == null)
        {
            Debug.LogError("materialIcons dictionary is null!");
            return null;
        }

        if (!craftingIcons.ContainsKey(craftingType))
        {
            Debug.LogWarning($"materialType '{craftingType}' not found in dictionary!");
            return null;
        }

        return craftingIcons[craftingType];
    }

    private void AddTooltipTriggers(GameObject materialSlot, CraftingType craftingType, int materialCount)
    {
        EventTrigger trigger = materialSlot.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = materialSlot.AddComponent<EventTrigger>();
        }
        trigger.triggers.Clear();

        string tooltipText = $"{materialSlot} material\nAmount: x{materialCount}";

        // Pointer Enter (Show Tooltip)
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((eventData) =>
        {
            if (TooltipManager.Instance != null)
            {
                TooltipManager.Instance.ShowTooltip(tooltipText, materialSlot.transform);
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
