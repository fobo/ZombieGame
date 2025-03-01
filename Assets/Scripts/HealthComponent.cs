using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{

    private DamageFlashController damageFlashController;

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100; // Maximum health.
    private float currentHealth;
    public GameObject damageTextNumber; // reference to the damage text number prefab.
    public Transform damageNumberSpawnPoint;

    [Header("Armor Settings")]
    [SerializeField] private float armor = 0; //armor value

    //  Event triggered when health reaches zero.
    public UnityEvent onHealthDepleted;

    //  Event triggered whenever health changes.
    public UnityEvent<float, float> onHealthChanged;

    private void Awake()
    {
        // Initialize current health to the max health.
        damageFlashController = GetComponent<DamageFlashController>();
        currentHealth = maxHealth;
        if (gameObject.CompareTag("Player"))
        {
            Debug.Log("HP: " + GetCurrentHealth() + "/" + maxHealth);

        }
    }
    private void Start()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            damageNumberSpawnPoint = transform.Find("damageNumberSpawnPoint");
        }
        if (gameObject.CompareTag("Player"))
        {
            HUDController.Instance?.UpdateHealthUI(currentHealth, maxHealth);
        }
    }

    /// <summary>
    /// Apply damage to the object.
    /// </summary>
    /// <param name="damageAmount">Amount of damage to apply.</param>
    /// <param name="damageSource">Object that caused the damage.</param>
    public void TakeDamage(Damage damageAmount)
    {
        if (damageAmount.damage <= 0 || currentHealth <= 0) return;


        currentHealth -= damageAmount.damage;

        // Clamp health to ensure it doesn't drop below 0.
        currentHealth = Mathf.Max(currentHealth, 0);

        // Notify listeners about the health change.
        onHealthChanged?.Invoke(currentHealth, maxHealth);

        // Trigger damage flash
        damageFlashController?.TriggerFlash();
        // If health reaches zero, invoke the health-depleted event.
        if (currentHealth == 0)
        {
            //Debug.Log($"{gameObject.name} has reached 0 hp");
            onHealthDepleted?.Invoke();
        }
        if (gameObject.CompareTag("Enemy"))
        {
            SpawnDamageNumber(damageAmount);
        }
        if (gameObject.CompareTag("Player"))
        {
            Debug.Log("HP: " + GetCurrentHealth() + "/" + maxHealth);
            HUDController.Instance?.UpdateHealthUI(currentHealth, maxHealth);
        }
    }

    public void SpawnDamageNumber(Damage damageAmount)
    {
        if (damageTextNumber == null || damageNumberSpawnPoint == null) return; // Ensure prefab and spawn point are assigned

        Vector3 spawnPos = damageNumberSpawnPoint.position; // Use the child object's position

        // Instantiate the damage number at the spawn point
        GameObject damageNumberInstance = Instantiate(damageTextNumber, spawnPos, Quaternion.identity);



        // Set the damage text value
        PopupDamage popupDamage = damageNumberInstance.GetComponent<PopupDamage>();

        //if critical hit, 
        if (damageAmount.isCritical)
        {
            popupDamage.textMeshProUGUI.color = Color.blue;
        }
        if (popupDamage != null && popupDamage.textMeshProUGUI != null)
        {
            popupDamage.textMeshProUGUI.SetText(Mathf.Round(damageAmount.damage).ToString());
        }

        Destroy(damageNumberInstance, 5f);
    }


    /// <summary>
    /// Heal the object.
    /// </summary>
    /// <param name="healAmount">Amount of health to restore.</param>
    public void Heal(float healAmount)
    {
        if (healAmount <= 0 || currentHealth <= 0) return;

        currentHealth += healAmount;

        // Clamp health to ensure it doesn't exceed max health.
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        // Notify listeners about the health change.
        onHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log("Healed " + healAmount);
        Debug.Log("Current HP: " + GetCurrentHealth());
        if (gameObject.CompareTag("Player"))
        {
            Debug.Log("HP: " + GetCurrentHealth() + "/" + maxHealth);
            HUDController.Instance?.UpdateHealthUI(currentHealth, maxHealth);
        }
    }

    /// <summary>
    /// Reset health to max health.
    /// </summary>
    public void ResetHealth()
    {
        currentHealth = maxHealth;

        if (gameObject.CompareTag("Player"))
        {
            Debug.Log("Reseting health?");
            onHealthChanged?.Invoke(currentHealth, maxHealth);
            Debug.Log("HP: " + GetCurrentHealth() + "/" + maxHealth);
            HUDController.Instance?.UpdateHealthUI(currentHealth, maxHealth);
        }
    }

    /// <summary>
    /// Get the current health value.
    /// </summary>
    public float GetCurrentHealth() => currentHealth;

    /// <summary>
    /// Get the maximum health value.
    /// </summary>
    public float GetMaxHealth() => maxHealth;

    public float GetArmorValue() => armor;
}
