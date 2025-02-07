using UnityEngine;
using TMPro; // Ensure you have TextMeshPro package installed

public class HUDController : MonoBehaviour
{
    public static HUDController Instance { get; private set; } // Singleton for easy access

    [Header("UI References")]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI healthText;
    public GameObject currentGun; // this is a reference to the current gun the player is holding.
    public GameObject gunHUD; // this is a ref to the gunHUD object that controls the gun animations in the bottom right corner of the screen.
    private Animator gunAnimator; // ref to the animator itself

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
    private void Start()
    {


        //on start, we set the currently equipped gun to whatever the currentGun is holding.
        //later on we will add weapon swapping, and a method will be called when we swap weapons to update here.
        if (gunHUD != null)
        {

            gunAnimator = gunHUD.GetComponent<Animator>();

            if (gunAnimator != null)
            {
                Debug.Log("Animator found on GunHUD!");
                //gunAnimator.runtimeAnimatorController
            }
            else
            {
                Debug.LogWarning("No Animator component found on GunHUD!");
            }

            currentGun = GameObject.FindWithTag("playerGun");
            if (currentGun == null)
            {
                Debug.LogWarning("Gun object not found in the scene!");
                
            }

            Gun gunScript = currentGun.GetComponent<Gun>(); // gets the Gun script
            if(gunScript == null){
                Debug.LogWarning("Could not find gun script.");
            }

                //we need to check if the gun script has a weapon data assigned to it, and the weapon data has an animation connected
            if(gunScript.weaponData != null && gunScript.weaponData.weaponAnimation != null){
                Debug.Log("Weapon animator set!");
                UpdateGunAnimationUI();
                //gunAnimator.runtimeAnimatorController = gunScript.weaponData.weaponAnimation;
            }
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

    public void UpdateGunAnimationUI(){
        Gun gunScript = currentGun.GetComponent<Gun>(); // gets the Gun script
        gunAnimator.runtimeAnimatorController = gunScript.weaponData.weaponAnimation;
    }

}
