using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MomentoSystem : MonoBehaviour
{
    public static MomentoSystem Instance { get; private set; } // Singleton


    public static event Action OnStatsChanged;

    private List<Momento> collectedMomentos = new List<Momento>();

    //example stats
    private int damageMultiplier = 0;
    private float apMultiplier = 1f;
    private int healthMultiplier = 0;
    private float fireRateMultiplier = 1f;
    private float reloadSpeedMultiplier = 1f;
    private float spreadMultiplier = 1f;
    private float moveSpeedMultiplier = 1f;
    private float treasureClassMultiplier = 1f;
    private int luckMultiplier = 1;
    private float criticalChanceMultiplier = 1f;
    private float stoppingPowerMultiplier = 1f;
    private float criticalDamageMultiplier = 1f;
    public static event Action OnMomentoReady;
    private void Awake()
    {

        // If we're in the main menu, don't persist this object
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Destroy(gameObject);
            return;
        }
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        OnMomentoReady?.Invoke();
    }

    /// <summary>
    /// Adds a new Momento and updates adjustments.
    /// </summary>
    public void AddMomento(Momento newMomento)
    {
        if (!collectedMomentos.Contains(newMomento))
        {
            collectedMomentos.Add(newMomento);
            InventorySystem.Instance.AddMomento(newMomento); // add momento to inventory
            //example stats
            // Apply stat multipliers
            damageMultiplier += newMomento.GetDamageMultiplier();
            apMultiplier *= newMomento.GetAPMultiplier();
            healthMultiplier += newMomento.GetHealthMultiplier();
            fireRateMultiplier /= newMomento.GetFireRateMultiplier();
            reloadSpeedMultiplier /= newMomento.GetReloadSpeedMultiplier();
            spreadMultiplier *= newMomento.GetSpreadMultiplier();
            moveSpeedMultiplier *= newMomento.GetMoveSpeedMultiplier();
            treasureClassMultiplier *= newMomento.GetTreasureClassMultiplier();
            luckMultiplier += newMomento.GetLuckMultiplier();
            criticalChanceMultiplier *= newMomento.GetCriticalChanceMultiplier();
            stoppingPowerMultiplier *= newMomento.GetStoppingPowerMultiplier();
            criticalDamageMultiplier *= newMomento.GetCriticalDamageMultiplier();

            // Apply special effect (if any)
            newMomento.ApplyEffect();
            OnStatsChanged?.Invoke();


//            Debug.Log($"Collected Momento: {newMomento.momentoName} - {newMomento.description}");
        }
    }

    // example getters
    // Getters for all stat adjustments
    public int GetDamageMultiplier() => damageMultiplier;
    public float GetAPMultiplier() => apMultiplier;
    public int GetHealthMultiplier() => healthMultiplier;
    public float GetFireRateMultiplier() => fireRateMultiplier;
    public float GetReloadSpeedMultiplier() => reloadSpeedMultiplier;
    public float GetSpreadMultiplier() => spreadMultiplier;
    public float GetMoveSpeedMultiplier() => moveSpeedMultiplier;
    public float GetTreasureClassMultiplier() => treasureClassMultiplier;
    public int GetLuckMultiplier() => luckMultiplier;
    public float GetCriticalChanceMultiplier() => criticalChanceMultiplier;
    public float GetStoppingPowerMultiplier() => stoppingPowerMultiplier;
    public float GetCriticalDamageMultiplier() => criticalDamageMultiplier;


    
}
