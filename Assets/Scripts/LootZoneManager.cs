using System.Collections.Generic;
using UnityEngine;

public class LootZoneManager : MonoBehaviour
{
    public static LootZoneManager Instance { get; private set; }

    [SerializeField] private List<LootZone> lootZones;
    private Dictionary<string, LootZone> lootZoneDictionary = new Dictionary<string, LootZone>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeLootZones();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeLootZones()
    {
        foreach (var zone in lootZones)
        {
            lootZoneDictionary[zone.shapeName] = zone;
        }
    }

    public GameObject GetRandomPrefabForZone(string shape, int tier)
    {
        if (!lootZoneDictionary.TryGetValue(shape, out LootZone lootZone))
        {
            Debug.LogWarning($"[LootZoneManager] No loot zone found for shape: {shape}");
            return null;
        }

        int maxTier = 4;
        float upgradeChance = 0.05f; //small initial chance to upgrade
        int finalTier = tier;

        // Optional: allow multi-upgrades
        if (finalTier < maxTier && Util.RollChance(upgradeChance))
        {
            finalTier++;
            //Debug.Log($"[LootZoneManager] Tier upgraded from {tier} to {finalTier}");
        }

        return lootZone.GetRandomPrefab(finalTier);
    }

}
