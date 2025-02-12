using System.Collections.Generic;
using UnityEngine;

public class MomentoSystem : MonoBehaviour
{
    public static MomentoSystem Instance { get; private set; } // Singleton

    private List<Momento> collectedMomentos = new List<Momento>();

    //example stats
    private float damageMultiplier = 1f;
    private float apMultiplier = 1f;
    private float healthMultiplier = 1f;
    private float fireRateMultiplier = 1f;
    private float reloadSpeedMultiplier = 1f;
    private float spreadMultiplier = 1f;
    private float moveSpeedMultiplier = 1f;
    private float treasureClassMultiplier = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Adds a new Momento and updates adjustments.
    /// </summary>
    public void AddMomento(Momento newMomento)
    {
        if (!collectedMomentos.Contains(newMomento))
        {
            collectedMomentos.Add(newMomento);

            //example stats
            // Apply stat multipliers
            damageMultiplier *= newMomento.GetDamageMultiplier();
            apMultiplier *= newMomento.GetAPMultiplier();
            healthMultiplier *= newMomento.GetHealthMultiplier();
            fireRateMultiplier *= newMomento.GetFireRateMultiplier();
            reloadSpeedMultiplier *= newMomento.GetReloadSpeedMultiplier();
            spreadMultiplier *= newMomento.GetSpreadMultiplier();
            moveSpeedMultiplier *= newMomento.GetMoveSpeedMultiplier();
            treasureClassMultiplier *= newMomento.GetTreasureClassMultiplier();

            // Apply special effect (if any)
            newMomento.ApplyEffect();

            Debug.Log($"Collected Momento: {newMomento.momentoName} - {newMomento.description}");
        }
    }
    
    // example getters
    // Getters for all stat adjustments
    public float GetDamageMultiplier() => damageMultiplier;
    public float GetAPMultiplier() => apMultiplier;
    public float GetHealthMultiplier() => healthMultiplier;
    public float GetFireRateMultiplier() => fireRateMultiplier;
    public float GetReloadSpeedMultiplier() => reloadSpeedMultiplier;
    public float GetSpreadMultiplier() => spreadMultiplier;
    public float GetMoveSpeedMultiplier() => moveSpeedMultiplier;
    public float GetTreasureClassMultiplier() => treasureClassMultiplier;
}
