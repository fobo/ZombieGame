using UnityEngine;
using TMPro;
using System; // Ensure you have TextMeshPro package installed
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class HUDController : MonoBehaviour
{
    public static HUDController Instance { get; private set; } // Singleton for easy access

    [Header("UI References")]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI healthText;
    public Image healthFillImage;
    public GameObject currentGun; // this is a reference to the current gun the player is holding.
    private StatsUIManager statsScript; // reference to the stats script so we can update the health easily

    private int currentHealthRef;
    private int maxHealthRef;
    private void Awake()
    {


        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }




    public static event Action OnHUDReady;
    public static event Action OnHealthChanged;
    private void Start()
    {





        EventBus.Instance.OnAmmoUpdated += UpdateAmmoUI;
        // All setup done
        OnHUDReady?.Invoke();
    }



    /// <summary>
    /// Updates the ammo count on the HUD.
    /// </summary>
    public void UpdateAmmoUI(int currentAmmo, int maxAmmo)
    {
        if (ammoText != null)
        {
            ammoText.text = $"Ammo: {currentAmmo} / {maxAmmo}";
        }
    }

    /// <summary>
    /// Updates the player's health on the HUD.
    /// </summary>
    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        //Debug.Log("Healthed");
        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }

        if (healthFillImage != null && maxHealth > 0)
        {
            healthFillImage.fillAmount = (float)currentHealth / maxHealth;
        }
        SetHealthMaxHealth(currentHealth, maxHealth);
        OnHealthChanged?.Invoke();
    }
    //set and hold references to the players health values whenever we update the UI
    private void SetHealthMaxHealth(int curr, int max)
    {
        currentHealthRef = curr;
        maxHealthRef = max;
    }

    public int GetHealth() => currentHealthRef;
    public int GetMaxHealth() => maxHealthRef;




}
