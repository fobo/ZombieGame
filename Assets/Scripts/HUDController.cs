using UnityEngine;
using TMPro; // Ensure you have TextMeshPro package installed

public class HUDController : MonoBehaviour
{
    public static HUDController Instance { get; private set; } // Singleton for easy access

    [Header("UI References")]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI healthText;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start() {
        if (healthText != null)
        {
            healthText.text = $"100 / 100";
        }

    }
    /// <summary>
    /// Updates the ammo count on the HUD.
    /// </summary>
    public void UpdateAmmoUI(int currentAmmo, int maxAmmo)
    {
        if (ammoText != null)
        {
            ammoText.text = $"{currentAmmo} / {maxAmmo}";
        }
    }

    /// <summary>
    /// Updates the player's health on the HUD.
    /// </summary>
    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }
}
