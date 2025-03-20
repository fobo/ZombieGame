using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; // For tooltip triggers

public class ConsumablesUIManager : MonoBehaviour
{
    public GameObject ammoSlotPrefab; // Assign in Unity Inspector
    public Transform consumablesPanel; // Assign in Unity Inspector

    [Header("Ammo Icons (Assign in Inspector)")]
    public Sprite NineMMIcon;
    public Sprite SevenSixTwoIcon;
    public Sprite ShotgunShellIcon;
    public Sprite FiveFiveSixIcon;
    public Sprite ThreeFiftySevenIcon;
    public Sprite RocketIcon;

    private Dictionary<AmmoType, Sprite> ammoIcons;

    private void Awake()
    {
        ammoIcons = new Dictionary<AmmoType, Sprite>
        {
            { AmmoType.NineMM, NineMMIcon },
            { AmmoType.SevenSixTwo, SevenSixTwoIcon },
            { AmmoType.ShotgunShell, ShotgunShellIcon },
            { AmmoType.FiveFiveSix, FiveFiveSixIcon },
            { AmmoType.ThreeFiftySeven, ThreeFiftySevenIcon },
            { AmmoType.Rocket, RocketIcon }
        };
    }

    private void OnEnable()
    {
        UpdateConsumablesUI();
    }

    public void UpdateConsumablesUI()
    {
        // Clear previous ammo slots
        foreach (Transform child in consumablesPanel)
        {
            Destroy(child.gameObject);
        }

        // Get all ammo counts from InventorySystem
        Dictionary<AmmoType, int> ammoCounts = InventorySystem.Instance.GetAmmoInventory();

        foreach (var ammoEntry in ammoCounts)
        {
            AmmoType ammoType = ammoEntry.Key;
            int ammoCount = ammoEntry.Value;

            // Instantiate a new ammo slot
            GameObject newAmmoSlot = Instantiate(ammoSlotPrefab, consumablesPanel);

            // Update UI components
            newAmmoSlot.transform.Find("AmmoIcon").GetComponent<Image>().sprite = GetAmmoIcon(ammoType);
            newAmmoSlot.transform.Find("AmmoCountText").GetComponent<TMP_Text>().text = $"x{ammoCount}";

            // Add tooltip triggers
            AddTooltipTriggers(newAmmoSlot, ammoType, ammoCount);
        }
    }

    private Sprite GetAmmoIcon(AmmoType ammoType)
    {
        if (ammoIcons == null)
        {
            Debug.LogError("GetAmmoIcon() - ammoIcons dictionary is null!");
            return null;
        }

        if (!ammoIcons.ContainsKey(ammoType))
        {
            Debug.LogWarning($"GetAmmoIcon() - AmmoType '{ammoType}' not found in dictionary!");
            return null;
        }

        return ammoIcons[ammoType];
    }

    private void AddTooltipTriggers(GameObject ammoSlot, AmmoType ammoType, int ammoCount)
    {
        EventTrigger trigger = ammoSlot.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = ammoSlot.AddComponent<EventTrigger>();
        }
        trigger.triggers.Clear();

        string tooltipText = $"{ammoType} Ammo\nAmount: x{ammoCount}";

        // Pointer Enter (Show Tooltip)
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((eventData) =>
        {
            if (TooltipManager.Instance != null)
            {
                TooltipManager.Instance.ShowTooltip(tooltipText, ammoSlot.transform);
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
