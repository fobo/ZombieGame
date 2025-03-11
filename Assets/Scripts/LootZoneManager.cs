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
        if (lootZoneDictionary.TryGetValue(shape, out LootZone lootZone))
        {
            return lootZone.GetRandomPrefab(tier);
        }

        Debug.LogWarning($"No loot zone found for shape {shape}!");
        return null;
    }
}
