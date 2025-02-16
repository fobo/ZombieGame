using UnityEngine;
using TMPro;
using System; // Ensure you have TextMeshPro package installed
using UnityEngine.SceneManagement;
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
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignReferences(); // Reassign gun reference on scene load
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
            if (gunScript == null)
            {
                Debug.LogWarning("Could not find gun script.");
            }

            //we need to check if the gun script has a weapon data assigned to it, and the weapon data has an animation connected
            if (gunScript.weaponData != null && gunScript.weaponData.weaponAnimation != null)
            {
                Debug.Log("Weapon animator set!");
                UpdateGunAnimationUI();
                //gunAnimator.runtimeAnimatorController = gunScript.weaponData.weaponAnimation;
            }
        }

        EventBus.Instance.OnAmmoUpdated += UpdateAmmoUI;
        EventBus.Instance.OnEquipWeapon += UpdateGunAnimationUI;

    }

    private void AssignReferences()
    {
        if (gunHUD != null)
        {
            gunAnimator = gunHUD.GetComponent<Animator>();

            if (gunAnimator != null)
                Debug.Log("Animator found on GunHUD!");
            else
                Debug.LogWarning("No Animator component found on GunHUD!");
        }

        currentGun = GameObject.FindWithTag("playerGun");

        if (currentGun == null)
        {
            Debug.LogWarning("Gun object not found in the scene!");
        }
        else
        {
            Gun gunScript = currentGun.GetComponent<Gun>();
            if (gunScript == null)
                Debug.LogWarning("Could not find Gun script.");

            if (gunScript?.weaponData?.weaponAnimation != null)
            {
                Debug.Log("Weapon animator set!");
                UpdateGunAnimationUI();
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
    public void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        Debug.Log("Healthed");
        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }

    public void UpdateGunAnimationUI()
    {
        Debug.Log("Changing weapon display animations");
        Gun gunScript = currentGun.GetComponent<Gun>(); // Get the Gun script
        gunAnimator.runtimeAnimatorController = gunScript.weaponData.weaponAnimation; // Set correct animation controller

        //  Get animation clips from the controller
        AnimatorOverrideController overrideController = new AnimatorOverrideController(gunAnimator.runtimeAnimatorController);
        gunAnimator.runtimeAnimatorController = overrideController;

        AnimationClip reloadClip = GetAnimationClip(overrideController, "Reload");
        AnimationClip fireClip = GetAnimationClip(overrideController, "Fire");

        if (reloadClip != null)
        {
            float reloadSpeed = reloadClip.length / gunScript.weaponData.reloadSpeed; //  Normalize speed
            gunAnimator.SetFloat("ReloadSpeed", reloadSpeed);
            Debug.Log($"Reload Animation: {reloadClip.name}, Default Length: {reloadClip.length}s, Adjusted Speed: {reloadSpeed}");
        }

        if (fireClip != null)
        {
            float fireSpeed = fireClip.length / gunScript.weaponData.fireRate; //  Normalize speed
            gunAnimator.SetFloat("FireSpeed", fireSpeed);
            Debug.Log($"Fire Animation: {fireClip.name}, Default Length: {fireClip.length}s, Adjusted Speed: {fireSpeed}");
        }
    }

    /// <summary>
    ///  Gets an animation clip by name from an AnimatorOverrideController.
    /// </summary>
    private AnimationClip GetAnimationClip(AnimatorOverrideController overrideController, string clipName)
    {
        foreach (var clipPair in overrideController.animationClips)
        {
            if (clipPair.name.Contains(clipName))
            {
                return clipPair;
            }
        }
        return null;
    }
}
