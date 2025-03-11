using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootZone
{
    public string shapeName;  // E.g., "L-Shape", "Square"
    
    [System.Serializable]
    public class TieredPrefabs
    {
        public int tier;
        public List<GameObject> prefabs;
    }

    public List<TieredPrefabs> tieredPrefabOptions = new List<TieredPrefabs>();

    public GameObject GetRandomPrefab(int tier)
    {
        var matchingTier = tieredPrefabOptions.Find(t => t.tier == tier);
        if (matchingTier != null && matchingTier.prefabs.Count > 0)
        {
            return matchingTier.prefabs[Random.Range(0, matchingTier.prefabs.Count)];
        }

        Debug.LogWarning($"No prefabs found for {shapeName} at tier {tier}!");
        return null;
    }
}
