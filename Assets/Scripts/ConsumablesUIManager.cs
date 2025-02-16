using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // For TextMeshPro support

public class ConsumablesUIManager : MonoBehaviour
{
    public GameObject ammoSlotPrefab; // Assign AmmoSlotPrefab in Unity Inspector
    public Transform consumablesPanel; // Assign the ConsumablesPanel in Unity Inspector

    [Header("Ammo Icons (Assign in Inspector)")]
    public Sprite NineMMIcon;
    public Sprite SevenSixTwoIcon;
    public Sprite ShotgunShellIcon;
    public Sprite FiveFiveSixIcon;
    public Sprite ThreeFiftySevenIcon;
    public Sprite RocketIcon;

    private Dictionary<AmmoType, Sprite> ammoIcons; // Cached dictionary for quick lookups

    private void Start()
    {
        // Initialize the sprite dictionary
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

    private void Awake()
    {
        if (ammoIcons == null)
        {
            ammoIcons = new Dictionary<AmmoType, Sprite>();
            Debug.LogWarning("ammoIcons dictionary was null and has been initialized!");
        }
    }


    private void OnEnable() // When inventory UI opens, update it
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

}
