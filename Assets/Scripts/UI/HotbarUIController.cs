using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HotbarUIController : MonoBehaviour
{
    [Header("Weapon Slot UI")]
    [SerializeField] private Image[] weaponIcons = new Image[9];
    [SerializeField] private GameObject[] blockers = new GameObject[9];
    [SerializeField] private GameObject[] selectionHighlights = new GameObject[9]; // Optional

    [Header("References")]
    [SerializeField] private PlayerController playerController;

    void Start()
    {
        UpdateHotbar(); // Initial draw
    }

    public void UpdateHotbar()
    {
        var ownedWeapons = InventorySystem.Instance.GetAllWeapons();
        var weaponSlots = playerController.weaponSlots;

        for (int i = 0; i < weaponSlots.Length; i++)
        {
            WeaponData weapon = weaponSlots[i];

            if (weapon != null)
            {
                //weaponIcons[i].sprite = weapon.weaponSprite;

                bool hasWeapon = ownedWeapons.ContainsKey(weapon.weaponName);

                // Make blocker transparent if owned, opaque if not
                SetBlockerAlpha(blockers[i], hasWeapon ? 0f : 1f);
            }
        }

        UpdateEquippedHighlight();
    }


    public void UpdateEquippedHighlight()
    {

        if (playerController.equippedGun == null || playerController.equippedGun.weaponData == null)
            return;
        string equippedWeaponName = playerController.equippedGun.weaponData.weaponName;

        for (int i = 0; i < playerController.weaponSlots.Length; i++)
        {
            Image highlightImage = selectionHighlights[i].GetComponent<Image>();
            if (highlightImage != null)
            {
                bool isSelected = playerController.weaponSlots[i].weaponName == equippedWeaponName;

                highlightImage.color = isSelected
                    ? new Color(1f, 0.84f, 0f, 1f) // Gold
                    : Color.white;
            }
        }
    }

    private void SetBlockerAlpha(GameObject blocker, float alpha)
    {
        Image img = blocker.GetComponent<Image>();
        if (img != null)
        {
            Color c = img.color;
            c.a = alpha;
            img.color = c;
        }
    }

}
